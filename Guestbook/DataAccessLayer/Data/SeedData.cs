using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace DataAccessLayer.Data
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new GuestbookContext(
                serviceProvider.GetRequiredService<DbContextOptions<GuestbookContext>>()))
            {
                if (context.Entries.Any())
                {
                    return;
                }

                context.Entries.AddRange(new Entry[]
               {
                    new Entry
                    {
                        Name = "CuckooBlue",
                        Email = "rgaazinho@dmarshallmarketing.com",
                        EntryTime = new DateTime(2021, 4, 1, 15, 21, 43),
                        Comment = "Don't want to close my eyes I don't want to fall asleep Cause I'd miss you babe And I don't want to miss a thing Cause even when I dream of you The sweetest dream will never do I'd still miss you babe And I don't want to miss a thing."
                    },
                    new Entry
                    {
                        Name = "lolita",
                        Email = "jqueen.of.senes.5@love97.ga",
                        EntryTime = new DateTime(2021, 4, 1, 19, 23, 32),
                        Comment = "I saw six men kicking and punching the mother-in-law. My neighbor said 'Are you going to help?' I said, 'No, Six should be enough.'"
                    },
                    new Entry
                    {
                        Name = "Phantonemesis",
                        Email = "ypc-boy12b@dailylonghorn.com",
                        EntryTime = new DateTime(2021, 4, 2, 9, 12, 16),
                        Comment = "I am ready to meet my Maker. Whether my Maker is prepared for the great ordeal of meeting me is another matter."
                    },
                    new Entry
                    {
                        Name = "Gobbeldy Gator",
                        Email = "hdesireeuro3@goreadit.site",
                        EntryTime = new DateTime(2021, 4, 2, 11, 21, 37),
                        Comment = "I told my wife the truth. I told her I was seeing a psychiatrist. Then she told me the truth: that she was seeing a psychiatrist, two plumbers, and a bartender."
                    },
                    new Entry
                    {
                        Name = "Mudpuppy",
                        Email = "wdma.brcs@viralchoose.com",
                        EntryTime = new DateTime(2021, 4, 2, 19, 13, 46),
                        Comment = "I'm always relieved when someone is delivering a eulogy and I realise I'm listening to it."
                    },
                    new Entry
                    {
                        Name = "hedgehound",
                        Email = "ha_chandru@hoanghamov.com",
                        EntryTime = new DateTime(2021, 4, 2, 19, 16, 23),
                        Comment = "The human body was designed by a civil engineer. Who else would run a toxic waste pipeline through a recreational area?"
                    },
                    new Entry
                    {
                        Name = "Ambrosaur",
                        Email = "obernadene3@gotcertify.com",
                        EntryTime = new DateTime(2021, 4, 2, 20, 21, 59),
                        Comment = "Sometimes I wonder if I really can. But then I think to myself, maybe I can't. But if I could, that would be good. Maybe it's all a lie?"
                    },
                    new Entry
                    {
                        Name = "Caterwhy",
                        Email = "qhoucine.kamal@gotcertify.com",
                        EntryTime = new DateTime(2021, 4, 2, 21, 57, 11),
                        Comment = "I like to wax my legs and stick the hair on my back. Why? Because it keeps my back warm. There's method in my madness."
                    },
                    new Entry
                    {
                        Name = "Ribbit Riot",
                        Email = "vciara2@juliman.me",
                        EntryTime = new DateTime(2021, 4, 3, 13, 8, 16),
                        Comment = "Look! In the sky. It's a bird, it's a plane. Or is it a hellicopter? No actually I think it is a bird. Or maybe I'm just seeing things. Who knows... After 10 shots of Whiskey things start to get a bit strange."
                    },
                    new Entry
                    {
                        Name = "CuckooBlue",
                        Email = "saif.ha@gmailwe.com",
                        EntryTime = new DateTime(2021, 4, 3, 13, 27, 18),
                        Comment = "People always told me be careful of what you do And dont go around breaking young girls' hearts And mother always told me be careful of who you love And be careful of what you do cause the lie becomes the truth."
                    },
                    new Entry
                    {
                        Name = "ninja",
                        Email = "2luuuuu@limez.ninja",
                        EntryTime = new DateTime(2021, 4, 3, 17, 52, 15),
                        Comment = "Look! In the sky. It's a bird, it's a plane. Or is it a hellicopter? No actually I think it is a bird. Or maybe I'm just seeing things. Who knows... After 10 shots of Whiskey things start to get a bit strange."
                    },
                    new Entry
                    {
                        Name = "Warewo",
                        Email = "8mouhcine.naci@indpro.tk",
                        EntryTime = new DateTime(2021, 4, 3, 23, 12, 55),
                        Comment = "Don't you find it Funny that after Monday(M) and Tuesday(T), the rest of the week says WTF?"
                    }
                });

                context.SaveChanges();
            }
        }
    }
}
