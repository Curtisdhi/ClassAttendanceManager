<!--Suggestions
1. Do a self joining form with attendanceOut.php and just insert this code after data verification.
2. Add error checking.
-->


<?php
//Pull info from attendance.php, trim, and strip tags.
//Very important if you want your SQL statements to function properly
$user = $_GET["UserName"];
$user = trim($user);
$user = strip_tags($user);
$number = $_GET["newNumber"];
$number = trim($number);
?>

<?php
//Establish a connection with your database("whatever.com", "username", "password", "database name");
$mydbconn = mysqli_connect("www.evilscriptmonkeys.com", "capstone", "capstone", "attendance");
//Check for error connecting if one happens display an error message.
if (mysqli_connect_errno()) 
{
  echo "Failed to connect to MySQL: " . mysqli_connect_error();
}
//To show what student and the number being inseted into the table
$var = "$user\n $number\n";
echo ($var);
//Pushing info into the students table.
mysqli_query($mydbconn, "UPDATE students 
						 SET StuNumber='$number'
						 WHERE UserName='$user'");
//Closing the connection.						 
mysqli_close($mydbconn);
//Print out student number was updated.
printf("Student Number Updated");
?>
