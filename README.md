Deep Space Mining, a game made for Ludum Dare 45 : http://ldjam.com/events/ludum-dare/45/deep-space-mining

The project was originally destined to be expanded into a full blown X-style game but was discontinued after the Jam as I moved on to the next project and looking for a job.

In Deep Space Mining you start with no coin, and no ship, aboard a freshly built station in the heart of a massive asteroid field. To begin you need to take a loan and use it
to buy your first ship. All ships can be piloted by you or an AI, but purchasing the AI pilot is a non negligeable added cost early on. By mining Ore or Ice from nearby asteroids,
you can build up your capital and keep investing more, and perhaps quickly get out of any debt if such is your preference.

The goal of the game is to reach a goal net worth. Careful though, as the more wealthy you become, the more of a plump target you become in the eyes of pirates.

This project is one of my many projects where the game ended up being more of a shell for me to try and build some sort of advanced system of, frankly, needless complexity.
In this case, I went pretty far with ship AI and how much it could be expanded. Each ship has a built in order queue system that could pretty easily be extended to a X-like order
queue interface for commanding subordinate ships. 

I also experimented with a different "looking for closest target" system for fight optimization than usual. The basic idea is that going for a "tile" / "cell" / "grid" based space
partitioning system to optimize querying for close-by objects COULD work, but would not be adapted to a Space game where "map" sizes could extend quite far out, even maybe
indefinitely. As such, I decided to go for a different strategy based on "Engagements" :

(1) -> At random, using some broad, "dumb" system, find out if anyone is currently in engagement range of an enemy without being part of an engagement already
(2) -> If one of the corresponding ships is already in an engagement, add the other one to that engagement. If neither are, then create a new engagement.
(3) -> An Engagement keeps track of what ship is part of it, sorted by "side", where the number of sides can be 2 or above. This gives ships looking for a target a narrowed
view of potential targets. Engagements also keep track of the average position of all ships (designated as the "Engagement Epicenter") and the average squared distance of all ships 
to that epicenter.
(4) -> When only one side remains, OR when the engagement becomes too "stretched", delete it (potentially rebuilding multiple new engagements for each "pocket" of fighting ships)

With that system, the "target lookup" is optimized on a per-engagement basis rather than a purely positional basis. It could be further optimized with "sub engagements" perhaps,
or a local space partioning with the "origin" of the partition being the engagement's epicenter. As far as I could test it, large fleet fights already had satisfactory performances.

