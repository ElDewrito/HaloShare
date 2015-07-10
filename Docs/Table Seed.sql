
-- Insert all the available Game Types
INSERT INTO GameTypes (InternalId, Name, InternalName, Description) VALUES
	(1, 'Capture the Flag', 'ctf', 'Invade your opponent''s stronghold, sieze their flag, and return it to your base to score.'),
	(2, 'Slayer', 'slayer', 'Kill your enemies.\nKill your friends'' enemies.\nKill your friends.'),
	(3, 'Oddball', 'oddball', 'Hold the skull to earn points. It''s like Hamlet, with guns.'),
	(4, 'King of the Hill', 'koth', 'Control the hill to earn points. Earn points to win. It''s good to be the King.'),
	(5, 'Juggernaut', 'juggernaut', 'If you meat the Juggernaut, kill the Juggernaut.'),
	(6, 'Territories', 'territories', 'Defend your territory and control the land. Teams earn points for territories they control.'),
	(7, 'Assault', 'assault', 'Carry your bomb to the enemy base, plant it, and defend it until it detonates.'),
	(8, 'Infection', 'infection', 'The timeless struggle of human versus zombie. If you die by a zombie''s hand, you join their ranks.'),
	(9, 'VIP', 'vip', 'One Player on each team is Very Important. They down the enemy VIP for points, but take care of your own.'),
	(10, 'Forge', 'forge', 'Collaborate in real time to edit and play variations of your favorite maps, from the subtle to the insane.');

-- Insert all the available Maps
INSERT INTO GameMaps (InternalId, Name, InternalName, IsOriginal, Description) VALUES
	(320, 'Guardian', 'guardian', 1, ''),
	(340, 'Valhalla', 'riverworld', 1, ''),
	(705, 'Diamondback', 's3d_avalanche', 0, ''),
	(703, 'Edge', 's3d_edge', 0, ''),
	(700, 'Reactor', 's3d_reactor', 0, ''),
	(31, 'Icebox', 's3d_turf', 0, '0');