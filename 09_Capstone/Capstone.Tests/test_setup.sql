DELETE reservation
DELETE site
DELETE campground
DELETE park

-- make a new park
INSERT INTO park (name, location, establish_date, area, visitors, description) VALUES ('Tech Elevator National Park', 'Cleveland, OH', '06/01/2016', 10, 120, 'The best school in the world')

DECLARE @newPark int = (SELECT @@Identity)

Insert into campground(park_id, name, open_from_mm, open_to_mm, daily_fee) Values (@newPark, 'Camp Brandon', 3, 9, 1000000.00)

DECLARE @newCamp int = (SELECT @@Identity)

insert into site (campground_id, site_number, max_occupancy, accessible, max_rv_length, utilities) VALUEs (@newCamp, 99, 10, 0, 1000, 0)

DECLARE @newSite int = (SELECT @@Identity)

insert into reservation (site_id, name, from_date, to_date, create_date) values(@newSite, 'Brandon', '06/23/2019', '06/27/2019', GETDATE())

declare @newReservation int = (select @@IDENTITY)

SELECT
	@newPark AS NewPark,
	@newCamp AS NewCamp,
	@newSite AS NewSite,
	@newReservation AS NewReservation