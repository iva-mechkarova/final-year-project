<?php
// Variables for connecting to DB
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "sounditout";

// Variables received from app
$id = $_POST["id"];
$ageGroup = $_POST["ageGroup"];

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);
// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

$sql = "INSERT INTO users(id, age_group) VALUES ('" . $id. "', '" . $ageGroup. "')";

if ($conn->query($sql) === TRUE) {
    echo "New user inserted successfully";

} else {
    echo "Error: " . $sql . "<br>" . $conn->error;
}
$conn->close();

?>