<!--Suggestions
1. Needs to have data verified whenever a student ID is input for change.
2. This is just functional code, needs to be made more visually appealing.
3. Student name select box shows up regardless of which button is pressed. Needs to be fixed.
-->

<?php
//Pull info from attendance.php, trim, and strip tags.
//Very important if you want your SQL statements to function properly
$course = $_GET["CourseName"];
$startTime = $_GET["BeginTime"];
$course = trim($course);
$course = strip_tags($course);
$startTime = trim($startTime);
$startTime = strip_tags($startTime);
?>

<html>
<meta http-equiv="Content-Type" content="text/html"; charset=utf-8">
<body>

<?php
//Establish a connection with your database("whatever.com", "username", "password", "database name");
$mydbconn = new mysqli("www.evilscriptmonkeys.com", "capstone", "capstone", "attendance");
//Check for error connecting if one happens display an error message.
if (mysqli_connect_errno())
{
	printf("connection failed: %s\n", mysqli_connect_error());
}
//If Create Form button clicked trigger this piece of code.
if ($_GET['action'] == 'Create Form')
{	
	//Create variables for ease of coding.
	$c =$course;
	$s = $startTime;
	//Build your SQL statement
	$sql = ("SELECT * FROM student WHERE CourseName = '$c' AND Time > '$s'");
	//Query database to get information
	$result = $mydbconn->query($sql);
	//Create and insert information into nice table.
	echo '<table border="1">';
	//Getting them headers so you know what you're seeing
	$classFields = $result->fetch_fields();
	foreach ($classFields as $classData)
	{
		echo "<th> $classData->name </th>";
	}
	//Followed by the actual row information
	while ($row = $result->fetch_assoc())
	{
		echo "<tr>";
		foreach($row as $col=>$val)
		{
			echo "<td> $val </td>";
		}
		echo "</tr>";
	}

}
?>
<!--To push information to changeID.php for verification of student number change.-->
<form method = "get" action="changeID.php">
<!--This needs to be changed so that it only appears when the subsequent button is pressed-->
Student Name: <select Name='UserName'>
<option value = "">---Select---</option>
<?php
//If Edit Student ID button clicked trigger this piece of code.
if ($_GET['action'] == 'Edit Student ID')
{
	//Create statement joining the two tables.
	$c =$course;
	$result = mysqli_query($mydbconn, "SELECT DISTINCT s2.UserName 
									   FROM student s1 
									   INNER JOIN students s2 
									   ON s1.UserName = s2.UserName 
									   WHERE s1.CourseName = '$c'");
	//Creating associative array of the row results.
	while(($row = mysqli_fetch_assoc($result)))
	{
		//Spitting out said information for a drop down box.
		echo "<option value=\" " . $row['UserName'] . "\">". $row['UserName'] . "</option>";
	}

}
?>
</select><br>
<!--Where to input new corrected student number.-->
Student Number Correction: <input type = "text" name="newNumber"><br>
<!--Behold the submit button-->
<input type="submit" name="action" value="Change" />
</body>
</html>

