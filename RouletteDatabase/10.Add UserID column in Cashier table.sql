Alter table Cashier
Add UserId uniqueidentifier not null;

Alter table Cashier
Add foreign key  (UserID) References aspnet_Users("UserId")
