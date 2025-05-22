--Users
CREATE TABLE Users (
    user_id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    email NVARCHAR(256) NOT NULL UNIQUE,
    password_hash NVARCHAR(512) NOT NULL,
    first_name NVARCHAR(100),
    last_name NVARCHAR(100),
    created_date DATETIME2(0) NOT NULL DEFAULT GETUTCDATE(),
    modified_date DATETIME2(0) NOT NULL DEFAULT GETUTCDATE()
);

-- ROLES TABLE
CREATE TABLE Roles (
    role_id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    role_name NVARCHAR(100) NOT NULL UNIQUE,
    description NVARCHAR(512)
);

-- USERSROLES JUNCTION TABLE
CREATE TABLE UsersRoles (
    user_id UNIQUEIDENTIFIER NOT NULL,
    role_id UNIQUEIDENTIFIER NOT NULL,
    assigned_date DATETIME2(0) DEFAULT GETUTCDATE(),

    CONSTRAINT PK_UsersRoles PRIMARY KEY (user_id, role_id),
    CONSTRAINT FK_UsersRoles_User FOREIGN KEY (user_id) REFERENCES Users(user_id) ON DELETE CASCADE,
    CONSTRAINT FK_UsersRoles_Role FOREIGN KEY (role_id) REFERENCES Roles(role_id) ON DELETE CASCADE
);

-- ARTISTS TABLE
CREATE TABLE Artists (
    artist_id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    first_name NVARCHAR(100),
    last_name NVARCHAR(100),
    gender NVARCHAR(20),
    birth_date DATE,
    nationality NVARCHAR(100),
    created_date DATETIME2(0) DEFAULT GETUTCDATE(),
    modified_date DATETIME2(0) DEFAULT GETUTCDATE(),
    biography NVARCHAR(MAX)
);

-- ARTIFACTS TABLE
CREATE TABLE Artifacts (
    artifact_id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    title NVARCHAR(255) NOT NULL,
    description NVARCHAR(MAX),
    date_start DATE,
    date_end DATE,
    date_display NVARCHAR(100),
    material NVARCHAR(100),
    dimension NVARCHAR(100),
    place_of_origin NVARCHAR(150),
    location NVARCHAR(150),
    longitude FLOAT,
    latitude FLOAT,
    image_url NVARCHAR(2048),
    created_date DATETIME2(0) DEFAULT GETUTCDATE(),
    modified_date DATETIME2(0) DEFAULT GETUTCDATE()
);

-- ARTIFACTSARTISTS JUNCTION TABLE
CREATE TABLE ArtifactsArtists (
    artifact_id UNIQUEIDENTIFIER NOT NULL,
    artist_id UNIQUEIDENTIFIER NOT NULL,

    CONSTRAINT PK_ArtifactsArtists PRIMARY KEY (artist_id, artifact_id),
    CONSTRAINT FK_ArtifactsArtists_Artifact FOREIGN KEY (artifact_id) REFERENCES Artifacts(artifact_id) ON DELETE CASCADE,
    CONSTRAINT FK_ArtifactsArtists_Artist FOREIGN KEY (artist_id) REFERENCES Artists(artist_id) ON DELETE CASCADE
);

-- TAGS TABLE
CREATE TABLE Tags (
    tag_id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    tag_name NVARCHAR(100) NOT NULL UNIQUE,
    tag_description NVARCHAR(512)
);

-- ARTIFACTSTAGS JUNCTION TABLE
CREATE TABLE ArtifactsTags (
    artifact_id UNIQUEIDENTIFIER NOT NULL,
    tag_id UNIQUEIDENTIFIER NOT NULL,

    CONSTRAINT PK_ArtifactsTags PRIMARY KEY (artifact_id, tag_id),
    CONSTRAINT FK_ArtifactsTags_Artifact FOREIGN KEY (artifact_id) REFERENCES Artifacts(artifact_id) ON DELETE CASCADE,
    CONSTRAINT FK_ArtifactsTags_Tag FOREIGN KEY (tag_id) REFERENCES Tags(tag_id) ON DELETE CASCADE
);

--Indexing
-- USERS Indexes
CREATE NONCLUSTERED INDEX IX_Users_Email ON Users(email)
INCLUDE (user_id, password_hash);

CREATE NONCLUSTERED INDEX IX_Users_Id ON Users(user_id);

-- ROLES Indexes
CREATE NONCLUSTERED INDEX IX_Roles_Name ON Roles(role_name)
INCLUDE (role_id);

CREATE NONCLUSTERED INDEX IX_Roles_Id ON Roles(role_id);

-- USERSROLES Indexes
CREATE NONCLUSTERED INDEX IX_UsersRoles_RoleUser ON UsersRoles(role_id, user_id);

CREATE NONCLUSTERED INDEX IX_UsersRoles_UserId ON UsersRoles(user_id)
INCLUDE (role_id);

-- ARTISTS Indexes
CREATE NONCLUSTERED INDEX IX_Artists_LastFirstName ON Artists(last_name, first_name)
INCLUDE (artist_id);

CREATE NONCLUSTERED INDEX IX_Artists_Birth ON Artists(birth_date)
INCLUDE (artist_id);

-- TAGS Indexes
CREATE NONCLUSTERED INDEX IX_Tags_Name ON Tags(tag_name)
INCLUDE (tag_id);

CREATE NONCLUSTERED INDEX IX_Tags_Id ON Tags(tag_id);

-- ARTIFACTS Indexes
CREATE NONCLUSTERED INDEX IX_Artifacts_Title ON Artifacts(title)
INCLUDE (artifact_id);

CREATE NONCLUSTERED INDEX IX_Artifacts_DateStart ON Artifacts(date_start)
INCLUDE (artifact_id);

CREATE NONCLUSTERED INDEX IX_Artifacts_Geo ON Artifacts(location, longitude, latitude)
INCLUDE (artifact_id);

CREATE NONCLUSTERED INDEX IX_Artifacts_QuickDisplay
    ON Artifacts(title, place_of_origin, date_display)
    INCLUDE (artifact_id, image_url);

CREATE NONCLUSTERED INDEX IX_Artifacts_Active
    ON Artifacts(date_display)
    INCLUDE (artifact_id)
    WHERE date_display IS NOT NULL;

-- ARTIFACTSARTISTS Indexes
CREATE NONCLUSTERED INDEX IX_ArtifactsArtists_Artifact ON ArtifactsArtists(artifact_id, artist_id);

CREATE NONCLUSTERED INDEX IX_ArtifactsArtists_Artist ON ArtifactsArtists(artist_id)
INCLUDE (artifact_id);

-- ARTIFACTSTAGS Indexes
CREATE NONCLUSTERED INDEX IX_ArtifactsTags_Artifact ON ArtifactsTags(artifact_id, tag_id);

CREATE NONCLUSTERED INDEX IX_ArtifactsTags_Tag ON ArtifactsTags(tag_id)
INCLUDE (artifact_id);
