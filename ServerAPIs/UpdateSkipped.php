<?php
// Variables for connecting to DB
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "sounditout";

// Variables received from app
$id = $_POST["id"];
$numRepeats = $_POST["numRepeats"];

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);
// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

$sql = "UPDATE asked_words SET skipped=TRUE WHERE id='" . $id. "'";

if ($conn->query($sql) === TRUE) {
    echo "Updated skipped for " . $id. " successfully";

} else {
    echo "Error: " . $sql . "<br>" . $conn->error;
}
$conn->close();

?>