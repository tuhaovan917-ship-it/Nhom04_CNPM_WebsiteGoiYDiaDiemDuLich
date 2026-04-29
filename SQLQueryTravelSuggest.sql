-- CREATE DATABASE
CREATE DATABASE TravelSuggest;
GO
USE TravelSuggest;
GO

-- 1. Users
CREATE TABLE [dbo].[Users] (
    [UserId] INT IDENTITY(1,1) PRIMARY KEY,
    [FullName] NVARCHAR(200) NOT NULL,
    [Email] NVARCHAR(255) NOT NULL UNIQUE,
    [PasswordHash] NVARCHAR(500) NULL, -- nếu dùng authentication local
    [Role] NVARCHAR(50) NOT NULL DEFAULT('User'),
    [IsEmailConfirmed] BIT NOT NULL DEFAULT(0),
    [CreatedAt] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    [UpdatedAt] DATETIME2 NULL
);
ALTER TABLE Users ADD Salt NVARCHAR(100) NULL;
GO

-- 2. PlaceTypes (loại hình: biển, núi, văn hóa...)
CREATE TABLE [dbo].[PlaceTypes] (
    [PlaceTypeId] INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(500) NULL
);
GO

-- 3. Places (địa điểm)
CREATE TABLE [dbo].[Places] (
    [PlaceId] INT IDENTITY(1,1) PRIMARY KEY,
    [Title] NVARCHAR(250) NOT NULL,
    [ShortDescription] NVARCHAR(500) NULL,
    [Description] NVARCHAR(MAX) NULL,
    [PlaceTypeId] INT NULL,
    [AvgRating] DECIMAL(3,2) NULL DEFAULT 0,
    [AvgCost] DECIMAL(18,2) NULL,
    [Latitude] DECIMAL(9,6) NULL,
    [Longitude] DECIMAL(9,6) NULL,
    [Address] NVARCHAR(500) NULL,
    [City] NVARCHAR(150) NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    [UpdatedAt] DATETIME2 NULL,
    CONSTRAINT FK_Places_PlaceTypes FOREIGN KEY (PlaceTypeId) REFERENCES PlaceTypes(PlaceTypeId)
);
GO

-- 4. PlaceImages (gallery)
CREATE TABLE [dbo].[PlaceImages] (
    [ImageId] INT IDENTITY(1,1) PRIMARY KEY,
    [PlaceId] INT NOT NULL,
    [ImageUrl] NVARCHAR(1000) NOT NULL,
    [IsPrimary] BIT NOT NULL DEFAULT 0,
    [SortOrder] INT NOT NULL DEFAULT 0,
    CONSTRAINT FK_PlaceImages_Places FOREIGN KEY (PlaceId) REFERENCES Places(PlaceId)
);
GO

-- 5. Tags & mapping (sở thích, tag)
CREATE TABLE [dbo].[Tags] (
    [TagId] INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL UNIQUE
);
GO

CREATE TABLE [dbo].[PlaceTags] (
    [PlaceTagId] INT IDENTITY(1,1) PRIMARY KEY,
    [PlaceId] INT NOT NULL,
    [TagId] INT NOT NULL,
    CONSTRAINT FK_PlaceTags_Places FOREIGN KEY (PlaceId) REFERENCES Places(PlaceId),
    CONSTRAINT FK_PlaceTags_Tags FOREIGN KEY (TagId) REFERENCES Tags(TagId)
);
GO

-- 6. Reviews / Comments
CREATE TABLE [dbo].[Reviews] (
    [ReviewId] INT IDENTITY(1,1) PRIMARY KEY,
    [PlaceId] INT NOT NULL,
    [UserId] INT NULL,
    [Rating] DECIMAL(2,1) NOT NULL, -- 0.5 - 5.0
    [Comment] NVARCHAR(MAX) NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Reviews_Places FOREIGN KEY (PlaceId) REFERENCES Places(PlaceId),
    CONSTRAINT FK_Reviews_Users FOREIGN KEY (UserId) REFERENCES Users(UserId)
);
GO

-- 7. FavoritePlaces
CREATE TABLE [dbo].[FavoritePlaces] (
    [FavoriteId] INT IDENTITY(1,1) PRIMARY KEY,
    [UserId] INT NOT NULL,
    [PlaceId] INT NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_FavoritePlaces_Users FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CONSTRAINT FK_FavoritePlaces_Places FOREIGN KEY (PlaceId) REFERENCES Places(PlaceId)
);
CREATE UNIQUE INDEX UX_Favorite_User_Place ON dbo.FavoritePlaces(UserId, PlaceId);
GO

-- 8. SearchHistory
CREATE TABLE [dbo].[SearchHistory] (
    [SearchId] INT IDENTITY(1,1) PRIMARY KEY,
    [UserId] INT NULL,
    [Keyword] NVARCHAR(500) NULL,
    [FiltersJson] NVARCHAR(MAX) NULL, -- lưu filters như JSON nếu cần
    [CreatedAt] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_SearchHistory_Users FOREIGN KEY (UserId) REFERENCES Users(UserId)
);
GO

-- 9. TravelPlans + PlanItems (kế hoạch)
CREATE TABLE [dbo].[TravelPlans] (
    [PlanId] INT IDENTITY(1,1) PRIMARY KEY,
    [UserId] INT NOT NULL,
    [Title] NVARCHAR(300) NULL,
    [StartDate] DATE NULL,
    [EndDate] DATE NULL,
    [MoveCost] DECIMAL(18,2) NULL DEFAULT 0,
    [StayCost] DECIMAL(18,2) NULL DEFAULT 0,
    [FoodCost] DECIMAL(18,2) NULL DEFAULT 0,
    [OtherCost] DECIMAL(18,2) NULL DEFAULT 0,
    [TotalCost] DECIMAL(18,2) NULL,
    [Itinerary] NVARCHAR(MAX) NULL, -- lưu gợi ý lịch trình dạng JSON hoặc text
    [CreatedAt] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_TravelPlans_Users FOREIGN KEY (UserId) REFERENCES Users(UserId)
);
GO

CREATE TABLE [dbo].[PlanItems] (
    [PlanItemId] INT IDENTITY(1,1) PRIMARY KEY,
    [PlanId] INT NOT NULL,
    [PlaceId] INT NULL,
    [DayNo] INT NULL,
    [Notes] NVARCHAR(1000) NULL,
    CONSTRAINT FK_PlanItems_TravelPlans FOREIGN KEY (PlanId) REFERENCES TravelPlans(PlanId),
    CONSTRAINT FK_PlanItems_Places FOREIGN KEY (PlaceId) REFERENCES Places(PlaceId)
);
GO

-- 10. PlanShare (tạo link chia sẻ)
CREATE TABLE [dbo].[PlanShares] (
    [ShareId] INT IDENTITY(1,1) PRIMARY KEY,
    [PlanId] INT NOT NULL,
    [ShareToken] NVARCHAR(100) NOT NULL UNIQUE,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    [ExpiredAt] DATETIME2 NULL,
    CONSTRAINT FK_PlanShares_TravelPlans FOREIGN KEY (PlanId) REFERENCES TravelPlans(PlanId)
);
GO

-- 11. CompareSessions (tùy chọn: lưu session so sánh)
CREATE TABLE [dbo].[CompareSessions] (
    [SessionId] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [UserId] INT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

CREATE TABLE [dbo].[CompareSessionItems] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [SessionId] UNIQUEIDENTIFIER NOT NULL,
    [PlaceId] INT NOT NULL,
    CONSTRAINT FK_CompareItems_Sessions FOREIGN KEY (SessionId) REFERENCES CompareSessions(SessionId),
    CONSTRAINT FK_CompareItems_Places FOREIGN KEY (PlaceId) REFERENCES Places(PlaceId)
);
GO

-- =========================================
-- 12. PasswordResets (Reset mật khẩu qua email)
-- =========================================
CREATE TABLE [dbo].[PasswordResets] (
    [ResetId] INT IDENTITY(1,1) PRIMARY KEY,
    [UserId] INT NOT NULL,
    [Token] NVARCHAR(100) NOT NULL,
    [ExpireAt] DATETIME2 NOT NULL,
    [IsUsed] BIT NOT NULL DEFAULT (0),
    [CreatedAt] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

    CONSTRAINT FK_PasswordResets_Users
        FOREIGN KEY (UserId) REFERENCES Users(UserId)
        ON DELETE CASCADE
);
GO

-- =========================================
-- 13. BLOG POSTS (Bài viết / Tin tức du lịch)
-- =========================================
CREATE TABLE [dbo].[BlogPosts] (
    [PostId] INT IDENTITY(1,1) PRIMARY KEY,
    [Title] NVARCHAR(300) NOT NULL,
    [Slug] NVARCHAR(300) NOT NULL UNIQUE, -- dùng cho URL SEO
    [Summary] NVARCHAR(500) NULL,         -- mô tả ngắn
    [Content] NVARCHAR(MAX) NOT NULL,     -- nội dung chi tiết
    [Thumbnail] NVARCHAR(1000) NULL,      -- ảnh đại diện (local hoặc link ngoài)
    [AuthorId] INT NOT NULL,              -- Admin viết bài
    [ViewCount] INT NOT NULL DEFAULT 0,
    [IsPublished] BIT NOT NULL DEFAULT 1,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    [UpdatedAt] DATETIME2 NULL,

    CONSTRAINT FK_BlogPosts_Users
        FOREIGN KEY (AuthorId) REFERENCES Users(UserId)
);
GO

-- =========================================
-- 14. BLOG - PLACE MAPPING (N-N)
-- =========================================
CREATE TABLE [dbo].[BlogPostPlaces] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [PostId] INT NOT NULL,
    [PlaceId] INT NOT NULL,

    CONSTRAINT FK_BlogPostPlaces_Posts
        FOREIGN KEY (PostId) REFERENCES BlogPosts(PostId)
        ON DELETE CASCADE,

    CONSTRAINT FK_BlogPostPlaces_Places
        FOREIGN KEY (PlaceId) REFERENCES Places(PlaceId)
        ON DELETE CASCADE
);
GO

-- =========================================
-- 15. Contact
-- =========================================
CREATE TABLE [dbo].[Contacts] (
    [ContactId] INT IDENTITY(1,1) PRIMARY KEY,
    [FullName] NVARCHAR(200) NOT NULL,
    [Email] NVARCHAR(255) NOT NULL,
    [Message] NVARCHAR(MAX) NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    [IsRead] BIT NOT NULL DEFAULT 0
);
GO


-- Tìm token nhanh khi reset mật khẩu
CREATE UNIQUE INDEX UX_PasswordResets_Token
ON dbo.PasswordResets(Token);
GO

-- Hỗ trợ truy vấn theo User
CREATE INDEX IDX_PasswordResets_User
ON dbo.PasswordResets(UserId);
GO

-- Optional: index hỗ trợ tìm kiếm nhanh theo Title/City
CREATE INDEX IDX_Places_Title ON dbo.Places(Title);
CREATE INDEX IDX_Places_City ON dbo.Places(City);
GO
