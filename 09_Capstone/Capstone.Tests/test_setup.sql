DELETE reservation
DELETE site
DELETE campground
DELETE park

-- make a new park
INSERT INTO park (name, location, establish_date, area, visitors, description) VALUES ('Tech Elevator National Park', 'Cleveland, OH', '06/01/2016', 10, 120, 'The best school in the world')

DECLARE @newPark int = (SELECT @@Identity)

Insert into campground(park_id, name, open_from_mm, open_to_mm, daily_fee) Values (@newPark, 'Camp Brandon', 3, 9, 1000000.00)

DECLARE @newCamp int = (SELECT @@Identity)




SELECT
	@newPark AS NewPark,
	@newCamp AS NewCamp