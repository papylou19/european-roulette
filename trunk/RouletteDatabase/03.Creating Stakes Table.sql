CREATE TABLE Stake
(
	Id int identity(1,1) primary key,
	ContractNumber bigint NOT NULL,
	CreateDate dateTime NOT NULL,
	Coefficient float NOT NULL,
	[Sum] int NOT NULL,	
	[Type] nvarchar(50) NOT NULL,
	Number tinyint NOT NULL,
	IsWinningTicket bit NOT NULL default(0),
	IsPayed bit NOT NULL default(0),
	PaymentDate datetime,
	GameId int NOT NULL,
	foreign key ( GameId ) references Game (Id),
);
Go