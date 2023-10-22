# Hướng dẫn về Cách Làm Việc với Yêu Cầu Merge vào Nhánh `develop`

## Quy Tắc Cơ Bản
1. **Luôn luôn làm việc trên một nhánh riêng:** Trước khi bạn bắt đầu làm việc trên một tính năng hoặc sửa lỗi, hãy tạo một nhánh mới từ `develop` và làm việc trên nhánh đó. Điều này giúp duy trì sự sạch sẽ của nhánh `develop` và giảm xung đột khi nhiều người cùng làm việc.

2. **Giữ nhánh `develop` luôn luôn ổn định:** Nhánh `develop` nên được duy trì với mã nguồn luôn chạy được mà không có lỗi lớn.Nếu develop có bất kỳ lỗi hoặc vấn đề lớn nào phải được sửa trước khi thêm mã mới.

3. **Kiểm tra mã của bạn trước khi đẩy:** Trước khi đẩy mã lên nhánh `develop`, hãy đảm bảo rằng mã của bạn đã được kiểm tra kỹ lưỡng. Chạy các kiểm tra đơn vị, tự động hóa và đảm bảo rằng không có lỗi trước khi thực hiện đẩy.

4. **Luôn luôn sử dụng một câu thông báo hợp lý:** Khi bạn đẩy mã lên nhánh `develop`, hãy thêm một câu thông báo rõ ràng và mô tả về những thay đổi bạn đã thực hiện. Điều này giúp cho mọi người hiểu được mục tiêu của thay đổi.

## Cách đẩy code lên git
1. ** Vào issue để check task của mình được giao
2. ** Cập nhật code từ nhánh develop về nhánh của mình bằng cách (git pull origin develop)
3. ** Tạo nhánh mới từ nhánh develop với tên là ["label của issue"] -["tên của issue"] (ví dụ: feature-login) bằng cách ( git chekout -b feature-login)
4. ** Làm việc trên nhánh của mình
5. ** Sau khi hoàn thành task thì cập nhật code từ nhánh develop về nhánh của mình bằng cách (git pull origin develop)
6. ** Kiểm tra code của mình có bị conflict với code trên nhánh develop không
7. ** Nếu có conflict thì giải quyết conflict
8. ** Nếu không có conflict thì đẩy code lên nhánh của mình bằng cách (git add .)
9. ** Sau đó commit code lên nhánh của mình bằng cách (git commit -m "#issue - description issue") (ví dụ: git commit -m "#1 - login")
10. ** Đẩy code lên nhánh của mình bằng cách (git push origin feature-login)
11. ** Tạo pull request từ nhánh của mình vào nhánh develop
12. ** Trong khi được phê duyệt thì tiếp tục làm các issue khác theo quy trình như trên (từ bước 2 nhưng tạo nhánh mới từ nhánh feature-login trước đó)
13. ** Sau khi được merge thì xóa nhánh của mình bằng cách (git branch -d feature-login)
14. ** Xoá nhánh trên remote bằng cách (git push origin --delete feature-login)
15. ** Làm tương tự với các issue khác


## Cách thêm data SQL
1. ** Xuất các dữ liệu mình thêm trong database ra viết vào file text là data.txt


## Lưu Ý
- **Hãy thường xuyên cập nhật và đồng bộ hóa với nhánh `develop` để theo kịp các thay đổi mới.**
- **Hãy đảm bảo rằng bạn phải viết thêm database vào file data.txt.**
- **Hãy đảm bảo rằng bạn đã kiểm tra mã của mình trước khi đẩy lên nhánh `develop`.**
- **Hãy đảm bảo rằng bạn đã thêm một câu thông báo hợp lý khi đẩy mã lên nhánh `develop`.**
- **Hãy đảm bảo rằng bạn đã xóa nhánh của mình trên remote sau khi được phê duyệt.**
- **Hãy đảm bảo rằng bạn đã xóa nhánh của mình trên local sau khi được phê duyệt.**

