----------------------------------------------------------------------------------
				Attendance Manager v3

Developers: Curtis Hicks, Dylan Nichols, Felix Manuel, Kevin Jackson, and Samuel Gillis 

		     Developed for Capstone 2015 for Dr. Kominek 
		        at Northeast State Community College
            Icons provided by simpleicon.com and picol.org

----------------------------------------------------------------------------------
Notes:
----------------------------------------------------------------------------------
This program can run as a portable program. A full installation is NOT required.

----------------------------------------------------------------------------------
Installation:
----------------------------------------------------------------------------------
Extract Instructor.Attendance.Manager.zip
Run the "install.bat" batch script
You may be asked for admin privileges. The script requires elevated privileges 
in order to install the program into C:/Program Files (x86), grant permissions, 
and create a shortcut for all users.
If not, try running the batch script as admin (right click the bat script and 
click "Run as Administrator") The script will automatically move the program 
directory into C:/Program Files (x86)/Instructor Attendance Manager
It will also configure the directory and the contents with the needed user permissions.
These permissions are for Authenticated Users, and they are given (RXW) Read, Execute, 
and Write. The program requires write for at least until the program's first run 
(after this it could be removed if desired) (The program requires write 
in order to encrypt the settings)
If successful, the script will then create a shortcut from 
C:/Program Files (x86)/Instructor Attendance Manager/Instructor Attendance Manager.exe 
to C:\Users\Public\Desktop\Instructor Attendance Manager.lnk

----------------------------------------------------------------------------------
Configuration:
----------------------------------------------------------------------------------
In order for the program to operate correctly, some changes are necessary.
The configuration file is located at 
C:/Program Files (x86)/Instructor Attendance Manager/DrKCrazyAttendance.config
The file is in XML format, the configuration values are located in the "setting" 
element with names that describe the setting’s purpose.
The "value" element is the value of the "setting" element, and its content's is 
the value that is to be modified.

If this file is freshly extracted from the zip, then most of these values are 
already preconfigured to the campus SQL server.
The only setting not preconfigured is "instructor" This value should be set to 
match the assigned instructor's name. This can also be set using the setting form
in the program.

A quick rundown of what each value does:
"Classroom":     The classroom where the program is installed
"SecurityPin":   A value that protects the settings from unauthorized modification, 
	also used for encryption purposes.
"SqlUsername":   The SQL username used to log into the SQL server
"SqlPassword":   The SQL user’s password used to log into the SQL server
"SqlServerAddr": The SQL server’s hostname
"SqlDatabase":   The SQL database name
"Encrypted":     A boolean value for the program to know when the settings 
	has been encrypted.
"Instructor":    This value repesents the instructor's name

----------------------------------------------------------------------------------
Reconfiguration:
----------------------------------------------------------------------------------
In the event the program needs to be reconfigured. (primarly the SQL settings), 
the "Encrypted" setting MUST be set to "False" in order to reconfigure the SQL 
settings. Modifying the classroom DOES NOT require "Encrypted" to be changed.

SqlUsername, SqlPassword, SqlServerAddr and, SqlDatabase will be encrypted after 
the program runs the first time with plain text values. 
Because of the nature of this, in order to modify a single value, all the values 
must be changed into plain text or the program will encrypt the already encrypted 
values and ultimately crashing the program.
It is recommended to have a copy of the configuration to redeploy the program with 
new settings.
