#!/bin/sh
kill "$(lsof -t -i:7001)"
sleep 1
kill "$(lsof -t -i:3001)"
sleep 1
kill "$(lsof -t -i:3000)"
sleep 1 

supervisorctl -c supervisord.conf update
supervisorctl -c supervisord.conf start all