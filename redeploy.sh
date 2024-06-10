#!/bin/sh

dotnet build
BUILD_SUCCESS=$?

if [ $BUILD_SUCCESS -eq 0 ]; then
  echo "Build succeeded, updating and starting all services with Supervisor"
else
  echo "Build failed, not updating or starting services"
  exit 1
fi

kill_if_running() {
  PORT=$1
  PID=$(lsof -t -i:$PORT)
  if [ -n "$PID" ]; then
    echo "Killing process $PID using port $PORT"
    kill -9 $PID
    echo "Process $PID killed"
  else
    echo "No process found using port $PORT"
  fi
}

# Attempt to kill processes on specified ports
kill_if_running 7001
sleep 1
kill_if_running 3001
sleep 1
kill_if_running 3000
sleep 1

# Update and start all services with Supervisor
supervisorctl -c supervisord.conf update
supervisorctl -c supervisord.conf start all
