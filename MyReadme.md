## Good commands

```bash
 supervisorctl -c supervisord.conf stop backend_listener
 kill $(lsof -t -i:7001)
 supervisorctl -c supervisord.conf start backend_listener
```