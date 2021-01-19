﻿using Microsoft.AspNetCore.Components;
using ServiceBase.Models.Articles;
using System.Threading.Tasks;

namespace ServiceBase.Pages.Articles
{
    public partial class Create
    {
        [Inject]
        public NavigationManager NavigationManagerInjector { get; set; }

        [Inject]
        public IArticleRepository ArticleRepository { get; set; }

        public Article Model { get; set; } = new Article();

        protected async Task btnSubmit_Click()
        {
            // 저장 로직 
            await ArticleRepository.AddArticleAsync(Model);

            // 리스트 페이지로 이동
            NavigationManagerInjector.NavigateTo("/Articles");
        }
    }
}
