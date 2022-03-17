<?php
// Variables for connecting to DB
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "sounditout";

// Variables received from app
$id = $_POST["id"];
$wordId = $_POST["wordId"];
$attempt = $_POST["attempt"];

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);
// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

$sql = "INSERT INTO attempts(id, word_id, attempt) VALUES ('" . $id. "', '" . $wordId. "', '" . $attempt. "')";

if ($conn->query($sql) === TRUE) {
  //echo "New attempt inserted successfully";
  $sql = "SELECT soundex_code FROM asked_words WHERE id='" . $wordId. "'";
  $result = $conn->query($sql);
  if ($result->num_rows > 0) {
    // output data of each row
    while($row = $result->fetch_assoc()) {
      echo $row["soundex_code"]. " " . soundex($attempt);
      break;
    }
    } else {
      echo "0 results";
    }
} else {
    echo "Error: " . $sql . "<br>" . $conn->error;
}
$conn->close();

?>