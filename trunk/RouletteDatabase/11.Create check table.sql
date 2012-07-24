create table [Check]
(
	ContractNumber bigint Primary key not null,
	PossibleWinning float not null,
	PossibleWinningString varchar(50) not null,
	GameID int not null,
    UserID uniqueidentifier not null,
    BoardCurrentStates  nvarchar(MAX) ,
    CreateDate DateTime not null,
    Foreign key(GameId) references Game(Id),
    Foreign key(UserId) references aspnet_Users(UserId),
    
);





