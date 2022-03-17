<?php
// Variables for connecting to DB
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "sounditout";

// Variables received from app
$id = $_POST["id"];
$userId = $_POST["userId"];
$targetWord = $_POST["targetWord"];
$difficulty = $_POST["difficulty"];

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);
// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

$sql = "INSERT INTO asked_words(id, user_id, target_word, difficulty) VALUES ('" . $id. "', '" . $userId. "', '" . $targetWord. "', '" . $difficulty. "')";

if ($conn->query($sql) === TRUE) {
    echo "New asked_word inserted successfully";

} else {
    echo "Error: " . $sql . "<br>" . $conn->error;
}
$conn->close();

?>