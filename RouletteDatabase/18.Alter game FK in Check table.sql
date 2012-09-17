ALTER TABLE [Check]
DROP CONSTRAINT FK__Check__GameID__74AE54BC;
Go

Alter table [Check]
add CONSTRAINT fk_game_id  foreign key(GameId) references [Game](Id) ON DELETE CASCADE;

