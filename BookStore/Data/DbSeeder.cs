using BookStore.Data.Models;
using BookStore.SharedComponents;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookStore.Data
{
    public class DbSeeder
    {

        public static void Seed(BookStoreDbContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            if (!dbContext.Users.Any())
            {
                CreateUser(dbContext, roleManager, userManager)
                   .GetAwaiter()
                   .GetResult();
            }
            if (!dbContext.Slides.Any())
            {
                CreateSlides(dbContext, userManager);
            }
            if (!dbContext.Books.Any())
            {
                CreateBook(dbContext, userManager);
            }
            if (!dbContext.Categories.Any())
            {
                CreatedCategory(dbContext);
            }
            if (!dbContext.BookTypes.Any())
            {
                CreateBookTypeByCategory(dbContext);
            }
            if (!dbContext.DownloadFormats.Any())
            {
                CreateFormat(dbContext);
            }
            if (!dbContext.Pages.Any())
            {
                CreatePage(dbContext,userManager);
            }
            if (!dbContext.Contacts.Any())
            {
                CreateContacts(dbContext,userManager);
            }
            if (!dbContext.Courses.Any())
            {
                CreateCourses(dbContext,userManager);
            }
            if (!dbContext.CourseCategories.Any())
            {
                CreateCourseCategory(dbContext, userManager);
            }
        }

        private static void CreateCourseCategory(BookStoreDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            var userId = userManager.FindByEmailAsync("vohung.it@gmail.com").Result.Id;
            List<CourseCategory> courseCategories = new List<CourseCategory>
            {
                new CourseCategory
                {
                    Id=Guid.NewGuid().ToString(),
                    Name="Lập trình",
                    CreateDate=DateTime.Now,
                    MetaName="lap-trinh",
                    Status=true,
                    UserId=userId
                },
                new CourseCategory
                {
                    Id=Guid.NewGuid().ToString(),
                    Name="Kinh doanh",
                    CreateDate=DateTime.Now,
                    MetaName="kinh-doanh",
                    Status=true,
                    UserId=userId
                }
            };
        }

        private static void CreateCourses(BookStoreDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            var userId = userManager.FindByEmailAsync("vohung.it@gmail.com").Result.Id;
            List<Course> courses = new List<Course>
            {
                new Course
                {
                    Id=Guid.NewGuid().ToString(),
                    Name="Lập trình .net core",
                    MetaName="lap-trinh-net-core",
                    AvatarData="/",
                    CreatedDate=DateTime.Now,
                    Description="Lập trình .net core",
                    UserId=userId,
                    Status=true,
                    Authors="NA",
                    SharedUrl="x"
                },
                new Course
                {
                    Id=Guid.NewGuid().ToString(),
                    Name="Thành thạo Linq trong vòng 7 tuần",
                    MetaName="thanh-thao-linq-trong-vong-7-tuan",
                    AvatarData="/",
                    CreatedDate=DateTime.Now,
                    Description="Lập trình .net core",
                    UserId=userId,
                    Status=true,
                    Authors="NA",
                    SharedUrl="x"
                }
            };
            courses.ForEach(x => dbContext.Courses.Add(x));
            dbContext.SaveChanges();
        }

        private static void CreateContacts(BookStoreDbContext dbContext,UserManager<ApplicationUser> userManager)
        {
            var userId = userManager.FindByEmailAsync("vohung.it@gmail.com").Result.Id;
            var contact = new Contact
            {
                Name = "Liên hệ",
                Address = "Phường Tân Thạnh - Tp.Tam Kỳ - Quảng Nam",
                Email = "vohung.it@gmail.com",
                Phone = "0169 565 5783",
                Status = true,
                UserId = userId
            };
            dbContext.Contacts.Add(contact);
            dbContext.SaveChanges();
        }
        private static void CreatePage(BookStoreDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            var userId = userManager.FindByEmailAsync("vohung.it@gmail.com").Result.Id;
            List<Page> pages = new List<Page>
            {
                new Page
                {
                    Id=Guid.NewGuid().ToString(),
                    Name="footer",
                    Content="footer",
                    CreatedDate=DateTime.Now,
                    UserId=userId,
                    Status=true
                },new Page
                {
                    Id=Guid.NewGuid().ToString(),
                    Name="contact",
                    Content="contact content seeder",
                    CreatedDate=DateTime.Now,
                    UserId=userId,
                    Status=true
                },new Page
                {
                    Id=Guid.NewGuid().ToString(),
                    Name="about",
                    Content="about content seeder",
                    CreatedDate=DateTime.Now,
                    UserId=userId,
                    Status=true
                }
            };
            pages.ForEach(x => dbContext.Pages.Add(x));
            dbContext.SaveChanges();
        }

        private static void CreateFormat(BookStoreDbContext dbContext)
        {
            List<DownloadFormat> formats = new List<DownloadFormat>
            {
                new DownloadFormat
                {
                    DisplayName="link1",
                    PdfLink="http://www.mediafire.com/file/7vl63g7szf5opcd/advanced-aspnet-ajax-server-controls-adam-calderon.pdf/file",

                }
            };
            formats.ForEach(x => dbContext.DownloadFormats.Add(x));
            dbContext.SaveChanges();
        }
        private static void CreateBookTypeByCategory(BookStoreDbContext dbContext)
        {
            List<BookType> type = new List<BookType>
            {
                new BookType
                {
                    Id=Guid.NewGuid().ToString(),
                    Name="Lập trình di động",
                    MetaName="Lập trình di động",
                    CategoryId=dbContext.Categories.Where(x=>x.Name=="Máy tính và Công nghệ").SingleOrDefault().Id,
                    Status=true,
                    CreatedDate=DateTime.Now,
                    ThumbnailUrl="http://www.mediafire.com/view/t8tn063vcglg808/android-300x300.png",
                    Description="Ebook về lập trình di động"
                },
                new BookType
                {
                    Id=Guid.NewGuid().ToString(),
                    Name="Lập trình website",
                    MetaName="Lập trình website",
                    CategoryId=dbContext.Categories.Where(x=>x.Name=="Máy tính và Công nghệ").SingleOrDefault().Id,
                    Status=true,
                    CreatedDate=DateTime.Now,
                    ThumbnailUrl="http://www.mediafire.com/view/dwlk4ed7vomaj02/tải%20xuống.png",
                    Description="Ebook về lập trình website"
                },
                new BookType
                {
                    Id=Guid.NewGuid().ToString(),
                    Name="Trí tuệ nhân tạo",
                    MetaName="Trí tuệ nhân tạo",
                    CategoryId=dbContext.Categories.Where(x=>x.Name=="Máy tính và Công nghệ").SingleOrDefault().Id,
                    Status=true,
                    CreatedDate=DateTime.Now,
                    ThumbnailUrl="http://www.mediafire.com/view/dwlk4ed7vomaj02/tải%20xuống.png",
                    Description="Ebook về lập trình trí tuệ nhân tạo"
                },
                 new BookType
                {
                     Id=Guid.NewGuid().ToString(),
                    Name="Kỹ năng giao tiếp",
                    MetaName="Kỹ năng giao tiếp",
                    CategoryId=dbContext.Categories.Where(x=>x.Name=="Phát triển kỹ năng").SingleOrDefault().Id,
                    Status=true,
                    CreatedDate=DateTime.Now,
                    ThumbnailUrl="http://www.mediafire.com/view/dwlk4ed7vomaj02/tải%20xuống.png",
                    Description="Ebook về lập kỹ năng giao tiếp kỹ năng thuyết trình"
                },
                new BookType
                {
                    Id=Guid.NewGuid().ToString(),
                    Name="Khai phá bản thân",
                    MetaName="Khai phá bản thân",
                    CategoryId=dbContext.Categories.Where(x=>x.Name=="Phát triển kỹ năng").SingleOrDefault().Id,
                    Status=true,
                    CreatedDate=DateTime.Now,
                    ThumbnailUrl="http://www.mediafire.com/view/dwlk4ed7vomaj02/tải%20xuống.png",
                    Description="Ebook khai phá bản thân"
                }, new BookType
                {
                    Id=Guid.NewGuid().ToString(),
                    Name="Toán học phổ thông",
                    MetaName="Toán học phổ thông",
                    CategoryId=dbContext.Categories.Where(x=>x.Name=="Toán học").SingleOrDefault().Id,
                    Status=true,
                    CreatedDate=DateTime.Now,
                    ThumbnailUrl="http://www.mediafire.com/view/dwlk4ed7vomaj02/tải%20xuống.png",
                    Description="Ebook toán học phổ thông"
                },
                new BookType
                {
                    Id=Guid.NewGuid().ToString(),
                    Name="Toán học ứng dụng",
                    MetaName="Toán học ứng dụng",
                    CategoryId=dbContext.Categories.Where(x=>x.Name=="Toán học").SingleOrDefault().Id,
                    Status=true,
                    CreatedDate=DateTime.Now,
                    ThumbnailUrl="http://www.mediafire.com/view/dwlk4ed7vomaj02/tải%20xuống.png",
                    Description="Ebook toán học ứng dụng"
                },
                 new BookType
                {
                     Id=Guid.NewGuid().ToString(),
                    Name="Chiến tranh Việt Nam",
                    MetaName="Chiến tranh Việt Nam",
                    CategoryId=dbContext.Categories.Where(x=>x.Name=="Lịch sử").SingleOrDefault().Id,
                    Status=true,
                    CreatedDate=DateTime.Now,
                    ThumbnailUrl="http://www.mediafire.com/view/dwlk4ed7vomaj02/tải%20xuống.png",
                    Description="Chiến tranh Việt Nam"
                },
                 new BookType
                {
                     Id=Guid.NewGuid().ToString(),
                    Name="Lịch sử thế giới",
                    MetaName="Lịch sử thế giới",
                    CategoryId=dbContext.Categories.Where(x=>x.Name=="Lịch sử").SingleOrDefault().Id,
                    Status=true,
                    CreatedDate=DateTime.Now,
                    ThumbnailUrl="http://www.mediafire.com/view/dwlk4ed7vomaj02/tải%20xuống.png",
                    Description="Lịch sử thế giới"
                },
                  new BookType
                {
                      Id=Guid.NewGuid().ToString(),
                    Name="Y khoa thường thức",
                    MetaName="Y khoa thường thức",
                    CategoryId=dbContext.Categories.Where(x=>x.Name=="Y khoa").SingleOrDefault().Id,
                    Status=true,
                    CreatedDate=DateTime.Now,
                    ThumbnailUrl="http://www.mediafire.com/view/dwlk4ed7vomaj02/tải%20xuống.png",
                    Description="Y khoa thường thức"
                },
                 new BookType
                {
                     Id=Guid.NewGuid().ToString(),
                    Name="Y học phương đông",
                    MetaName="Y học phương đông",
                    CategoryId=dbContext.Categories.Where(x=>x.Name=="Y khoa").SingleOrDefault().Id,
                    Status=true,
                    CreatedDate=DateTime.Now,
                    ThumbnailUrl="http://www.mediafire.com/view/dwlk4ed7vomaj02/tải%20xuống.png",
                    Description="Y học phương đông"
                },
                 new BookType
                {
                     Id=Guid.NewGuid().ToString(),
                    Name="Y học phương tây",
                    MetaName="Y học phương tây",
                    CategoryId=dbContext.Categories.Where(x=>x.Name=="Y khoa").SingleOrDefault().Id,
                    Status=true,
                    CreatedDate=DateTime.Now,
                    ThumbnailUrl="http://www.mediafire.com/view/dwlk4ed7vomaj02/tải%20xuống.png",
                    Description="Y học phương tây"
                }, new BookType
                {
                    Id=Guid.NewGuid().ToString(),
                    Name="Khởi nghiệp",
                    MetaName="Khởi nghiệp",
                    CategoryId=dbContext.Categories.Where(x=>x.Name=="Kinh doanh và Khởi nghiệp").SingleOrDefault().Id,
                    Status=true,
                    CreatedDate=DateTime.Now,
                    ThumbnailUrl="http://www.mediafire.com/view/dwlk4ed7vomaj02/tải%20xuống.png",
                    Description="Khởi nghiệp"
                },
                 new BookType
                {
                     Id=Guid.NewGuid().ToString(),
                    Name="Luật doanh nghiệp",
                    MetaName="Luật doanh nghiệp",
                    CategoryId=dbContext.Categories.Where(x=>x.Name=="Pháp luật").SingleOrDefault().Id,
                    Status=true,
                    CreatedDate=DateTime.Now,
                    ThumbnailUrl="http://www.mediafire.com/view/dwlk4ed7vomaj02/tải%20xuống.png",
                    Description="Luật doanh nghiệp"
                },new BookType
                {
                    Id=Guid.NewGuid().ToString(),
                    Name="Luật nhà nước",
                    MetaName="Luật nhà nước",
                    CategoryId=dbContext.Categories.Where(x=>x.Name=="Pháp luật").SingleOrDefault().Id,
                    Status=true,
                    CreatedDate=DateTime.Now,
                    ThumbnailUrl="http://www.mediafire.com/view/dwlk4ed7vomaj02/tải%20xuống.png",
                    Description="Luật nhà nước"
                },

            };
            type.ForEach(x => dbContext.BookTypes.Add(x));
            dbContext.SaveChanges();
        }

        public static void CreatedCategory(BookStoreDbContext dbContext)
        {
            List<Category> cate = new List<Category> {
                new Category
                {
                    Id=Guid.NewGuid().ToString(),
                    Name="Máy tính và Công nghệ",
                    MetaName=StringHelper.ToUnsignString("Máy tính và Công nghệ"),
                    CreatedDate=DateTime.Now,
                    Status=true
                },
                new Category
                {
                    Id=Guid.NewGuid().ToString(),
                    Name="Phát triển kỹ năng",
                    MetaName=StringHelper.ToUnsignString("Phát triển kỹ năng"),
                    CreatedDate=DateTime.Now,
                    Status=true
                },
                new Category
                {
                    Id=Guid.NewGuid().ToString(),
                    Name="Toán học",
                    MetaName=StringHelper.ToUnsignString("Toán học"),
                    CreatedDate=DateTime.Now,
                    Status=true
                },new Category
                {
                    Id=Guid.NewGuid().ToString(),
                    Name="Lịch sử",
                    MetaName=StringHelper.ToUnsignString("Lịch sử"),
                    CreatedDate=DateTime.Now,
                    Status=true
                },new Category
                {
                    Id=Guid.NewGuid().ToString(),
                    Name="Y khoa",
                    MetaName=StringHelper.ToUnsignString("Y khoa"),
                    CreatedDate=DateTime.Now,
                    Status=true
                },new Category
                {
                    Id=Guid.NewGuid().ToString(),
                    Name="Kinh doanh và Khởi nghiệp",
                    MetaName=StringHelper.ToUnsignString("Kinh doanh và khởi nghiệp"),
                    CreatedDate=DateTime.Now,
                    Status=true
                },new Category
                {
                    Id=Guid.NewGuid().ToString(),
                    Name="Tiểu sử và Hồi ký",
                    MetaName=StringHelper.ToUnsignString("Tiểu sử và hồi ký"),
                    CreatedDate=DateTime.Now,
                    Status=true
                },new Category
                {
                    Id=Guid.NewGuid().ToString(),
                    Name="Pháp luật",
                    MetaName=StringHelper.ToUnsignString("Pháp luật"),
                    CreatedDate=DateTime.Now,
                    Status=true
                },new Category
                {
                    Id=Guid.NewGuid().ToString(),
                    Name="Nấu ăn",
                    MetaName=StringHelper.ToUnsignString("Nấu ăn"),
                    CreatedDate=DateTime.Now,
                    Status=true
                }
            };
            cate.ForEach(c => dbContext.Categories.Add(c));
            dbContext.SaveChanges();
        }
        public static void CreateBook(BookStoreDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            var userId = userManager.FindByEmailAsync("vohung.it@gmail.com").Result.Id;
            IEnumerable<Book> books = new List<Book>();

        }
        public static void CreateSlides(BookStoreDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            var userId =  userManager.FindByEmailAsync("vohung.it@gmail.com").Result.Id;
            List<Slide> slides = new List<Slide>
            {
                 new Slide
            {
                Id=Guid.NewGuid().ToString(),
                Name = "slide 1",
                UserId = userId,
                Authors = "Hồ Chí Minh",
                Content = "Lao động trí óc mà không lao động chân tay, chỉ biết lý luận mà không biết thực hành thì cũng là trí thức có một nửa. Vì vậy, cho nên các cháu trong lúc học lý luận cũng phải kết hợp với thực hành và tất cả các ngành khác đều phải: lý luận kết hợp với thực hành, học tập kết hợp với lao động.",
                DisplayOrder = 1,
                Image = "http://www.mediafire.com/convkey/2b3c/cfd3i2sc2t4tr4izg.jpg",
                IsChoose = true,
                Status = true
            },
                 new Slide
            {
                     Id=Guid.NewGuid().ToString(),
                Name = "slide 2",
                UserId = userId,
                Authors = "Frank Herbert ",
                Content = "Người ta chỉ học được từ sách và các ví dụ rằng một số thứ có thể làm được. Học hỏi thực sự yêu cầu bạn phải thực hiện chúng.",
            DisplayOrder = 1,
                Image = "http://www.mediafire.com/convkey/6546/66qp4fak56c84qtzg.jpg",
                IsChoose = true,
                Status = true
            }
        };
            slides.ForEach(x => dbContext.Slides.Add(x));
            dbContext.SaveChanges();
        }
        public static async Task CreateUser(BookStoreDbContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            string role_Administrator = "Administrator";
      
            if (!await roleManager.RoleExistsAsync(role_Administrator))
            {
                await roleManager.CreateAsync(new IdentityRole(role_Administrator));
            }
         
            //create the "Admin" ApplicationUser account
            var user_Admin = new ApplicationUser
            {

                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = "administrator",
                Email = "vohung.it@gmail.com",
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                Address="Tiên hiệp - tiên phước - quảng nam",
                Status=true,
                PhoneNumber="0374564297",
                UrlAvatar="default",
                DisplayName="administrator"
                
            };
            if (await userManager.FindByNameAsync(user_Admin.UserName) == null)
            {
              IdentityResult result =   await userManager.CreateAsync(user_Admin, "pass4Admin");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user_Admin,role_Administrator);
                    await dbContext.SaveChangesAsync();
                }
             
            }
   

        }

    }
}
