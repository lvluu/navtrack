import traceback
import argparse
import json
import sqlite3
import socket
import datetime

from get_locations import getReports, retrieveICloudKey

def check_database(con: sqlite3.Connection):
    c = con.cursor()
    c.execute("SELECT count(name) FROM sqlite_master WHERE type='table' AND name='track_table'")

    if c.fetchone()[0] == 1:
        print("Reusing existing table, Warning, checking for if uniqueness is enforced hasn't been implemented")
    else:
        c.execute("""
        CREATE TABLE track_table(
            id TEXT,
            lat FLOAT,
            lon FLOAT,
            timestamp INTEGER,
            conf INTEGER,
            status INTEGER,
            UNIQUE(id,lat,lon,timestamp,conf,status)
        );
        """)
    con.commit()

def send_alematics_messages(messages, host, port, separate_connections=False, max_byte_size=None):
    for message in messages:
        try:
            message_data = message.encode('utf-8')
            if max_byte_size is not None and len(message_data) > max_byte_size:
                print(f"Skipping Alematics message as it exceeds the maximum byte size of {max_byte_size} bytes.")
                continue
            with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
                s.connect((host, int(port)))
                s.sendall(message_data)
        except Exception as e:
            print(f'Error sending Alematics message: {str(e)}')
            traceback.print_exc()

def convert_timestamp(timestamp):
    # Convert Unix timestamp to UTC datetime object
    dt_object = datetime.datetime.utcfromtimestamp(timestamp)

    # Format datetime object as a string in the desired format
    formatted_time = dt_object.strftime('%Y%m%d%H%M%S')

    # Print original and converted timestamps for verification
    print(f"Original timestamp: {timestamp}, Converted timestamp (UTC): {formatted_time}")

    return formatted_time

def get_reports(prefix='', database=None, interval=None, key=None, path=None, separate_connections=False, max_byte_size=None, host=None, port=None):
    if database is None:
        raise Exception('Need to specify a database')

    try:
        con = sqlite3.connect(database=database)
        check_database(con)
    except sqlite3.OperationalError as err:
        print("You may need to run `sudo chown user .` on the directory")
        raise err

    if interval is None:
        interval = 365 * 24 * 60 * 60

    try:
        ordered = getReports(prefix=prefix, hours=4 * interval / (60 * 60), key=key, doPrint=False, path=path)
        print(f"data: {ordered}")
        cur = con.cursor()
        alematics_messages = []
        new_point = []
        for res in ordered:
            cur.execute(
                'SELECT COUNT(*) FROM track_table WHERE id = ? AND lat = ? AND lon = ? AND timestamp = ? AND conf = ? and status = ?',
                (
                    res['key'],
                    res['lat'],
                    res['lon'],
                    res['timestamp'],
                    res['conf'],
                    res['status'],
                ),
            )
            count = cur.fetchone()[0]
            if count == 0:
                # Convert timestamp to desired format (UTC)
                formatted_timestamp = convert_timestamp(res['timestamp'])
                
                # Construct Alematics protocol message
                alematics_message = f"$T,2,64,{res['key']},{formatted_timestamp},{formatted_timestamp},{res['lat']},{res['lon']},0,0,0,{res['conf']},7,99,0,0.000,{res['status']},0,"
                alematics_messages.append(alematics_message)
                new_point.append((res['key'], res['lat'], res['lon'], res['timestamp'], res['conf'], res['status']))
        cur.executemany(
            "INSERT OR IGNORE INTO track_table (id, lat, lon, timestamp, conf, status) VALUES(?, ?, ?, ?, ?, ?)",
            new_point)
        print(f'Inserted {cur.rowcount} rows')
        con.commit()
        try:
            print(f"Alematics messages: {alematics_messages}")
            send_alematics_messages(alematics_messages, host, port, separate_connections, max_byte_size)
        except Exception as e:
            print(f'Error sending Alematics messages: {str(e)}')
            traceback.print_exc()
        return alematics_messages
    except Exception as e:
        print(f'Error occurred: {str(e)}')
        traceback.print_exc()
    finally:
        con.close()

if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument('-p', '--prefix', help='only use keyfiles starting with this prefix', default='')
    parser.add_argument('-d', '--database', help='sqlite database path')
    parser.add_argument('-k', '--key', help='iCloud decryption key, if not supplied will use specified password or prompt for password', default=None)
    parser.add_argument('--path', help='Path to directory containing the key file/s', default='./')
    parser.add_argument('--password', help='keychain password, if not supplied will use specified key or prompt for password ', default=None)
    parser.add_argument('--hours', help='interval hours', default=None)
    parser.add_argument('--days', help='interval days', default=None)
    parser.add_argument('-m', '--minutes', help='interval minutes', default=None)
    parser.add_argument('--repeat', help='Repeat the request every interval', default=False)
    parser.add_argument('--host', help='host address', default='192.110.2.2')
    parser.add_argument('--port', help='port number', default=54239)
    parser.add_argument('-s', '--separate-connections', help='Separate connections for each JSON object', action='store_true')
    parser.add_argument('-b', '--max-byte-size', help='Maximum byte size for each packet', type=int, default=None)

    args = parser.parse_args()

    if any([args.minutes, args.hours, args.days]):
        minutes = float(args.minutes) if args.minutes is not None else 0
        hours = float(args.hours) if args.hours is not None else 0
        days = float(args.days) if args.days is not None else 0
        interval = int(60 * (minutes + 60 * (hours + 24 * days)))
        print(f"Will repeat every {interval} seconds")
    else:
        interval = None
        print("No interval specified, will run once and exit")

    iCloud_decryptionkey = args.key if args.key is not None else retrieveICloudKey(args.password)
    get_reports(prefix=args.prefix, database=args.database, interval=interval, key=iCloud_decryptionkey, path=args.path, separate_connections=args.separate_connections, max_byte_size=args.max_byte_size, host=args.host, port=args.port)