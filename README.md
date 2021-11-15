# HAdmin
admin for test vuetify

1. Install Docker and pull SQL Server 2019 image
https://docs.microsoft.com/zh-tw/sql/linux/quickstart-install-connect-docker?view=sql-server-ver15&pivots=cs1-bash
password: @password

2. Add a database called "HWAdmin"

3. pull this repository and run: dotnet ef database update

4. enter /ClientApp folder and run: npm run build

5. at repository folder run: dotnet watch run

6. open browser: https://localhost:5001
