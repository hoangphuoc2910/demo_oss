<?php
include 'config.php';
$method = $_SERVER['REQUEST_METHOD'];
$input = json_decode(file_get_contents("php://input"));

// 1. GET: Lấy danh sách
if ($method === 'GET') {
    $stmt = $conn->prepare("SELECT * FROM products ORDER BY id DESC");
    $stmt->execute();
    echo json_encode($stmt->fetchAll(PDO::FETCH_ASSOC));
}
// 2. POST: Thêm mới
elseif ($method === 'POST') {
    if (!empty($input->name) && !empty($input->price)) {
        $sql = "INSERT INTO products (name, price, description) VALUES (?, ?, ?)";
        $stmt = $conn->prepare($sql);
        $stmt->execute([$input->name, $input->price, $input->description ?? '']);
        echo json_encode(["success" => true]);
    }
}
// 3. PUT: Sửa (Cập nhật)
elseif ($method === 'PUT') {
    if (!empty($input->id) && !empty($input->name)) {
        $sql = "UPDATE products SET name=?, price=?, description=? WHERE id=?";
        $stmt = $conn->prepare($sql);
        $stmt->execute([$input->name, $input->price, $input->description ?? '', $input->id]);
        echo json_encode(["success" => true]);
    }
}
// 4. DELETE: Xóa
elseif ($method === 'DELETE') {
    $id = $_GET['id'] ?? null;
    if ($id) {
        $stmt = $conn->prepare("DELETE FROM products WHERE id=?");
        $stmt->execute([$id]);
        echo json_encode(["success" => true]);
    }
}

?>