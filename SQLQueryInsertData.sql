-- 1) PlaceTypes – Loại hình du lịch
INSERT INTO PlaceTypes (Name, Description)
VALUES
(N'Biển', N'Du lịch biển, hải sản, tắm biển'),
(N'Núi', N'Du lịch leo núi, khí hậu mát mẻ'),
(N'Văn hoá', N'Du lịch văn hoá, lịch sử'),
(N'Nghỉ dưỡng', N'Resort & nghỉ dưỡng'),
(N'Thành phố', N'Thành phố sôi động, đa dạng hoạt động');

-- 2) Tags – Thẻ gợi ý
INSERT INTO Tags (Name)
VALUES
(N'Check-in'),
(N'Ẩm thực'),
(N'Thiên nhiên'),
(N'Giá rẻ'),
(N'Gia đình'),
(N'Cặp đôi'),
(N'Phiêu lưu');

-- 3) Places – 8 địa điểm mẫu
INSERT INTO Places (Title, ShortDescription, Description, PlaceTypeId, AvgRating, AvgCost, Latitude, Longitude, Address, City)
VALUES
(N'Đà Nẵng', N'Thành phố biển xanh, hiện đại', N'Đà Nẵng là thành phố đáng sống với biển Mỹ Khê, Bà Nà Hills, và nhiều cảnh đẹp.', 1, 4.7, 2500000, 16.0545, 108.2022, N'Biển Mỹ Khê, Q.Sơn Trà', N'Đà Nẵng'),
(N'Nha Trang', N'Thành phố biển nổi tiếng', N'Nha Trang nổi tiếng với biển đẹp, VinWonders, lặn biển...', 1, 4.5, 2200000, 12.2388, 109.1967, N'Trần Phú, TP.Nha Trang', N'Khánh Hòa'),
(N'Đà Lạt', N'Thành phố hoa, khí hậu mát mẻ', N'Đà Lạt có hồ Xuân Hương, đồi chè, cảnh quan lãng mạn...', 2, 4.8, 3000000, 11.9404, 108.4583, N'Trung tâm TP.Đà Lạt', N'Lâm Đồng'),
(N'Vũng Tàu', N'Thành phố biển gần TP.HCM', N'Du lịch biển, hải sản, tượng chúa Kitô...', 1, 4.3, 1800000, 10.411, 107.1362, N'Bãi Sau, TP.Vũng Tàu', N'Bà Rịa - Vũng Tàu'),
(N'Hà Nội', N'Thủ đô ngàn năm văn hiến', N'Văn hoá - lịch sử, hồ Hoàn Kiếm, phố cổ...', 5, 4.6, 2000000, 21.0278, 105.8342, N'Hoàn Kiếm', N'Hà Nội'),
(N'Huế', N'Kinh thành cổ kính', N'Đại Nội, lễ hội và di tích lịch sử', 3, 4.4, 1900000, 16.4637, 107.5909, N'Trung tâm TP.Huế', N'Thừa Thiên Huế'),
(N'Phú Quốc', N'Đảo ngọc tuyệt đẹp', N'Biển, lặn biển, nghỉ dưỡng cao cấp', 4, 4.9, 4200000, 10.2289, 103.9607, N'Dương Đông', N'Kiên Giang'),
(N'Sapa', N'Núi rừng hùng vĩ', N'Fansipan, bản Cát Cát, trekking', 2, 4.7, 2800000, 22.3381, 103.8448, N'TT.Sa Pa', N'Lào Cai');

-- 4) PlaceImages – Ảnh mẫu
DECLARE @i INT = 1;
WHILE @i <= 8
BEGIN
    INSERT INTO PlaceImages (PlaceId, ImageUrl, IsPrimary)
    VALUES
    (@i, 'https://picsum.photos/seed/' + CAST(@i AS NVARCHAR) + '/800/500', 1),
    (@i, 'https://picsum.photos/seed/' + CAST(@i+100 AS NVARCHAR) + '/800/501', 0),
    (@i, 'https://picsum.photos/seed/' + CAST(@i+200 AS NVARCHAR) + '/800/502', 0),
    (@i, 'https://picsum.photos/seed/' + CAST(@i+300 AS NVARCHAR) + '/800/503', 0);

    SET @i = @i + 1;
END;

-- 5) PlaceTags – gán tag cho địa điểm
-- Đà Nẵng
INSERT INTO PlaceTags (PlaceId, TagId) VALUES (1, 1), (1, 3), (1, 6);
-- Nha Trang
INSERT INTO PlaceTags (PlaceId, TagId) VALUES (2, 1), (2, 3), (2, 4);
-- Đà Lạt
INSERT INTO PlaceTags (PlaceId, TagId) VALUES (3, 3), (3, 6), (3, 5);
-- Vũng Tàu
INSERT INTO PlaceTags (PlaceId, TagId) VALUES (4, 1), (4, 4);
-- Hà Nội
INSERT INTO PlaceTags (PlaceId, TagId) VALUES (5, 3), (5, 2);
-- Huế
INSERT INTO PlaceTags (PlaceId, TagId) VALUES (6, 2), (6, 3);
-- Phú Quốc
INSERT INTO PlaceTags (PlaceId, TagId) VALUES (7, 1), (7, 3), (7, 4);
-- Sapa
INSERT INTO PlaceTags (PlaceId, TagId) VALUES (8, 3), (8, 7);

-- 6) Users mẫu
INSERT INTO Users (FullName, Email, PasswordHash, IsEmailConfirmed)
VALUES
(N'Nguyễn Văn A', 'user1@gmail.com', 'HASHED_PASSWORD', 1),
(N'Trần Thị B', 'user2@gmail.com', 'HASHED_PASSWORD', 1);

-- 7) FavoritePlaces
INSERT INTO FavoritePlaces (UserId, PlaceId)
VALUES
(1, 1),
(1, 3),
(1, 7),
(2, 2),
(2, 4);

-- 8) Reviews
INSERT INTO Reviews (PlaceId, UserId, Rating, Comment)
VALUES
(1, 1, 5.0, N'Tuyệt vời, biển đẹp và sạch'),
(1, 2, 4.5, N'Dịch vụ tốt'),
(3, 1, 4.8, N'Không khí mát, cảnh đẹp'),
(7, 1, 5.0, N'Đảo đẹp, đáng đi'),
(2, 2, 4.2, N'Nước biển rất đẹp và trong'),
(4, 2, 4.0, N'Hải sản ngon');

-- 9) SearchHistory
INSERT INTO SearchHistory (UserId, Keyword, FiltersJson)
VALUES
(1, N'Du lịch biển', N'{"budget":3000000,"type":"biển"}'),
(1, N'Gần TP.HCM', N'{"distance":200}'),
(2, N'Du lịch nghỉ dưỡng', N'{"type":"nghỉ dưỡng"}');

-- 10) TravelPlans + PlanItems + PlanShares
-- TravelPlans
INSERT INTO TravelPlans
(UserId, Title, StartDate, EndDate, MoveCost, StayCost, FoodCost, OtherCost, TotalCost, Itinerary)
VALUES
(1, N'Kế hoạch Đà Lạt 3N2Đ', '2025-01-12', '2025-01-14', 600000, 1500000, 600000, 300000, 3000000,
 N'Ngày 1: Di chuyển – Nhận phòng – Khám phá trung tâm\nNgày 2: Tham quan\nNgày 3: Trả phòng'),

(1, N'Kế hoạch Vũng Tàu cuối tuần', '2024-12-20', '2024-12-22', 400000, 800000, 400000, 200000, 1800000,
 N'Ngày 1: Di chuyển – Biển Bãi Sau\nNgày 2: Tượng Chúa – Hải sản');

-- PlanItems
INSERT INTO PlanItems (PlanId, PlaceId, DayNo, Notes)
VALUES
(1, 3, 1, N'Thăm hồ Xuân Hương'),
(1, 3, 2, N'Đồi chè – trung tâm'),
(2, 4, 1, N'Biển Bãi Sau'),
(2, 4, 2, N'Ăn hải sản');

-- PlanShares
INSERT INTO PlanShares (PlanId, ShareToken)
VALUES
(1, 'share-12345'),
(2, 'share-88888');

-- Admins
INSERT INTO Users (FullName, Email, PasswordHash, Role, IsEmailConfirmed)
VALUES
(N'Quản trị viên', 'admin@travelsuggest.com', 'HASHED_PASSWORD', 'Admin', 1);
GO

update Users
set Salt = '7foyz7sj2UAGInC4OX+1fg=='
where Email = 'admin@travelsuggest.com'

update Users
set PasswordHash = '49636977b582f0368121b5343d2b85b88c70bd3b15e37fd7fab93bb56e770ff4' -- Password: admin123
where Email = 'admin@travelsuggest.com'

-- Blogs
INSERT INTO BlogPosts
(Title, Slug, Summary, Content, Thumbnail, AuthorId)
VALUES
(
    N'Top 5 địa điểm du lịch Đà Lạt không thể bỏ lỡ',
    N'top-5-dia-diem-du-lich-da-lat',
    N'Gợi ý những địa điểm nổi bật nhất khi du lịch Đà Lạt.',
    N'
    <p>Đà Lạt là thành phố du lịch nổi tiếng với khí hậu mát mẻ quanh năm.</p>
    <p>Dưới đây là 5 địa điểm bạn không nên bỏ lỡ khi đến Đà Lạt:</p>
    <ul>
        <li>Hồ Xuân Hương</li>
        <li>Đỉnh Langbiang</li>
        <li>Thung lũng Tình Yêu</li>
        <li>Nhà thờ Con Gà</li>
        <li>Vườn hoa thành phố</li>
    </ul>
    ',
    'https://picsum.photos/800/400',
    1
),
(
    N'Kinh nghiệm du lịch Phú Quốc tự túc',
    N'kinh-nghiem-du-lich-phu-quoc-tu-tuc',
    N'Chia sẻ kinh nghiệm đi Phú Quốc tiết kiệm và hiệu quả.',
    N'
    <p>Phú Quốc là thiên đường biển đảo của Việt Nam.</p>
    <p>Bạn nên đi vào mùa khô từ tháng 11 đến tháng 4.</p>
    ',
    'phuquoc.jpg',
    1
);
GO

-- BlogPostPlaces
-- Blog 1: Top 5 địa điểm du lịch Đà Lạt
INSERT INTO BlogPostPlaces (PostId, PlaceId)
VALUES
(1, 3), -- Đà Lạt
(1, 8); -- Sapa (cùng loại hình núi, khí hậu mát)
GO

-- Blog 2: Kinh nghiệm du lịch Phú Quốc tự túc
INSERT INTO BlogPostPlaces (PostId, PlaceId)
VALUES
(2, 7), -- Phú Quốc
(2, 1), -- Đà Nẵng
(2, 2), -- Nha Trang
(2, 4); -- Vũng Tàu
GO

INSERT INTO BlogPostPlaces (PostId, PlaceId)
VALUES
(3, 5), -- Hà Nội
(3, 6); -- Huế
GO
