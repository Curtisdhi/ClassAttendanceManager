<!--Suggestions
1. Needs to be made more visually appealing with some css.
2. Write some javascript that looks for the first dropdown 
   to be selected which then triggers a listener to only
   get the times that are associated with the selected class instead of
   all the times available.
-->
<html>
<head>
<meta http-equiv="Content-Type" content="text/html"; charset=utf-8">
<title></title>
<body>
<!--Create form for submission in order to either show a list of the punches or change student numbers-->
<form method="get" action="attendanceOut.php">
Course Name: <select Name='CourseName'>
<option value="">--- Select ---</option>

<?php
//Establish a connection with your database("whatever.com", "username", "password", "database name");
$mydbconn = new mysqli("www.evilscriptmonkeys.com", "capstone", "capstone", "attendance");
//Check for error connecting if one happens display an error message.
if (mysqli_connect_errno())
{
	//Said error message.
	printf("connection failed: %s\n", mysqli_connect_error());
}
//Creating your mysqli query for sorting data.
$result = mysqli_query($mydbconn, "SELECT DISTINCT CourseName  FROM courses ORDER BY CourseName");
//Creating associative array of the row results.
while(($row = mysqli_fetch_assoc($result)))
{
	//Spit out information into a drop down selection box. 
	echo "<option value=\" " . $row['CourseName'] . "\">". $row['CourseName'] . "</option>";
}	
?>
</select><br><br><br>

Start Time: <select Name='BeginTime'>
<option value="">--- Select ---</option>

<?php
//Wash, rinse, repeat to create the next dropdown box.

$result2 = mysqli_query($mydbconn, "SELECT DISTINCT StartTime FROM courses ORDER BY StartTime");

while(($row = mysqli_fetch_assoc($result2)))
{
	echo "<option value=\" " . $row['StartTime'] . "\">". $row['StartTime'] . "</option>";
}
?>
</select><br><br><br>


<!--For creating your two submit buttons to differentiate between what you want to do.-->
<input type="submit" name="action" value="Create Form" />
<input type="submit" name="action" value="Edit Student ID" />
</form>


</body>
</html>