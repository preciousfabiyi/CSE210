class Program
{
    static void Main(string[] args)
    {
        // Create videos
        Video video1 = new Video("Unboxing the New Pro Laptop 2026", "TechReviewHub", 742);
        video1.AddComment(new Comment("CoffeeAndCode", "Great review! The keyboard feel was what I was most curious about."));
        video1.AddComment(new Comment("ZaraM", "Bought mine yesterday — battery life is even better than advertised."));
        video1.AddComment(new Comment("DigitalNomadDave", "Would love to see a comparison with last year's model."));
        video1.AddComment(new Comment("PixelPusher99", "That product placement at 4:20 was smooth but I noticed it lol."));

        Video video2 = new Video("How to Make Authentic Neapolitan Pizza at Home", "KitchenCraft", 1238);
        video2.AddComment(new Comment("FlourPower", "Finally a recipe that doesn't call for a stand mixer. Thank you!"));
        video2.AddComment(new Comment("MarcoBianchi", "As an Italian I approve — except you need San Marzano tomatoes only."));
        video2.AddComment(new Comment("WeekendChef42", "My family absolutely loved this. Made it twice already."));

        Video video3 = new Video("10 Minute Full Body Stretch — Morning Routine", "MoveWithMaya", 614);
        video3.AddComment(new Comment("BackPainBegone", "My back feels so much better after doing this three days in a row."));
        video3.AddComment(new Comment("YogaBee", "Love that the activewear brand fits seamlessly without being annoying about it."));
        video3.AddComment(new Comment("EarlyBirdElla", "Been doing this before work for two weeks — game changer for my energy levels."));
        video3.AddComment(new Comment("MovementMike", "Could you do a version specifically for desk workers?"));

        Video video4 = new Video("Building a Budget Gaming PC in 2026", "RigReport", 2105);
        video4.AddComment(new Comment("FPSFreak", "The GPU segment at 18 minutes was super helpful — that's where I always get lost."));
        video4.AddComment(new Comment("SavingsGamer", "Followed this guide and saved over $200 vs the pre-built I was looking at."));
        video4.AddComment(new Comment("QuietFanClub", "Please do a follow-up on cable management — mine is a disaster."));

        // Collect all videos in a list
        List<Video> videos = new List<Video> { video1, video2, video3, video4 };

        // Display all videos and their comments
        foreach (Video video in videos)
        {
            Console.WriteLine("==============================================");
            Console.WriteLine($"Title:    {video.Title}");
            Console.WriteLine($"Author:   {video.Author}");
            Console.WriteLine($"Length:   {video.LengthSeconds} seconds");
            Console.WriteLine($"Comments: {video.GetNumberOfComments()}");
            Console.WriteLine("----------------------------------------------");

            foreach (Comment comment in video.GetComments())
            {
                Console.WriteLine($"  {comment.CommenterName}: {comment.Text}");
            }

            Console.WriteLine();
        }
    }
}