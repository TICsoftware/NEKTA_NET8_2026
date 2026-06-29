using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nekta_MVC.Routes
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(this WebApplication app)
        {
            // ✅ Custom routes first


            app.MapControllerRoute(
                          name: "TestDirectory",
                          pattern: "test-directory",
                          defaults: new { controller = "TestDirectory", action = "Index" }
                      );




            app.MapControllerRoute(
                           name: "eventsandcme",
                           pattern: "academy-and-research/events-and-cme",
                           defaults: new { controller = "Events", action = "Index", title = "events-and-cme" }
                       );

                app.MapControllerRoute(
                    name: "LoadMoreSearch",
                    pattern: "Search/LoadMoreSearch",
                    defaults: new { controller = "Search", action = "LoadMoreSearch" }
                );

            app.MapControllerRoute(
                name: "search",
                pattern: "search/{id?}",
                defaults: new { controller = "Search", action = "Index" }
            );



 app.MapControllerRoute(
               name: "LoadMoreAnnualArticles",
               pattern: "Updates/LoadMoreAnnualArticles",
               defaults: new { controller = "Updates", action = "LoadMoreAnnualArticles" }
           );

            app.MapControllerRoute(
               name: "updates",
               pattern: "about-cop/updates",
               defaults: new { controller = "Updates", action = "Index", title = "updates" }
           );

            app.MapControllerRoute(
                name: "mediainside",
                pattern: "about-cop/updates/media/{id?}",
                defaults: new { controller = "Updates", action = "MediaInside" }
                );

            app.MapControllerRoute(
                name: "publicationsandresources",
                pattern: "academy-and-research/publications-and-resources",
                defaults: new { controller = "AcademyResearch", action = "PublicationsResources", title = "publications-and-resources" }
            );

            app.MapControllerRoute(
                name: "partnerscollaborators",
                pattern: "about-cop/partners-and-collaborators",
                defaults: new { controller = "About", action = "PartnersCollaborators", title = "partners-and-collaborators" }
            );

            app.MapControllerRoute(
                           name: "careers",
                           pattern: "about-cop/careers",
                           defaults: new { controller = "About", action = "Careers", title = "careers" }
                       );

            app.MapControllerRoute(
                name: "aboutcop",
                pattern: "about-cop",
                defaults: new { controller = "About", action = "About_Cop" }
            );

            app.MapControllerRoute(
                name: "testsandservices",
                pattern: "tests-and-services",
                defaults: new { controller = "TestsServices", action = "Index" }
            );

            app.MapControllerRoute(
                           name: "caseconsults",
                           pattern: "clinical-expertise/case-consults",
                           defaults: new { controller = "ClinicalExpertise", action = "CaseConsults", title = "case-consults" }
                       );

            app.MapControllerRoute(
                name: "ourteam",
                pattern: "clinical-expertise/our-team",
                defaults: new { controller = "Ourteam", action = "Index", title = "our-team" }
            );
            app.MapControllerRoute(
             name: "meetfellow",
             pattern: "/academy-and-research/fellowships-and-training/meet-our-fellows",
             defaults: new { controller = "Ourteam", action = "Index", title = "meet-our-fellows" }
         );
            app.MapControllerRoute(
                name: "contactus",
                pattern: "contactus",
                defaults: new { controller = "pagearticle", action = "Index", id = "contact" }
            );

            app.MapControllerRoute(
                name: "legal-disclaimer",
                pattern: "legal-disclaimer",
                defaults: new { controller = "pagearticle", action = "article", id = "legal-disclaimer" }
            );
              app.MapControllerRoute(
                name: "sitemap",
                pattern: "sitemap",
                defaults: new { controller = "pagearticle", action = "article", id = "sitemap" }
            );
            app.MapControllerRoute(
                name: "fellowshipsandtraining",
                pattern: "academy-and-research/fellowships-and-training",
                defaults: new { controller = "AcademyResearch", action = "FellowshipsTraining", title = "fellowships-and-training" }
            );

            app.MapControllerRoute(
                name: "dranitaborges",
                pattern: "academy-and-research/dr-anita-borges",
                defaults: new { controller = "AcademyResearch", action = "AnitaBorges", title = "dr-anita-borges" }
            );


            app.MapControllerRoute(
                name: "forpatients",
                pattern: "for-patients",
                defaults: new { controller = "ForPatients", action = "Index" }
            );

            app.MapControllerRoute(
                name: "Error",
                pattern: "Error",
                defaults: new { controller = "pagearticle", action = "Error" }
            );

            // ✅ Area / Admin route (before default)
            app.MapControllerRoute(
                name: "manage",
                pattern: "Manage/{action=Login}/{id?}",
                defaults: new { controller = "Manage" }
            );

            // ✅ Default route LAST
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );
        }
    }
}