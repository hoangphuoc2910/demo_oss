<?php
// --- MỞ KHÓA CORS (BẮT BUỘC ĐỂ SOMEE GỌI VÀO ĐƯỢC) ---
header("Access-Control-Allow-Origin: *"); // Dấu * nghĩa là cho phép tất cả
header("Access-Control-Allow-Headers: Content-Type");
header("Access-Control-Allow-Methods: GET, POST, PUT, DELETE, OPTIONS");
header("Content-Type: application/json; charset=UTF-8");

if ($_SERVER['REQUEST_METHOD'] === 'OPTIONS') {
    http_response_code(200);
    exit();
}

// --- KẾT NỐI DB ---
$host = "sql203.infinityfree.com"; // Xem trong MySQL Details trên InfinityFree
$db_name = "if0_40579233_db_shop";      // Tên Database
$username = "if0_40579233";           // Username
$password = "Phuoc09087879";       // Password vPanel

try {
    $conn = new PDO("mysql:host=$host;dbname=$db_name;charset=utf8", $username, $password);
    $conn->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
} catch (PDOException $e) {
    echo json_encode(["message" => "Lỗi DB: " . $e->getMessage()]);
    exit();
}
