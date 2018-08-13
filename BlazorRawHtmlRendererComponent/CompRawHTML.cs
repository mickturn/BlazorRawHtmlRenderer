using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.RenderTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazorRawHtmlRendererComponent
{
    public class CompRawHTML : BlazorComponent
    {
        [Parameter]
        public string RawHTML { get; set; }

        [Parameter]
        private Action<string> RawHTMLChanged { get; set; }



        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {

          

            LupParser lp1 = new LupParser(RawHTML);


            if (lp1.Validate())
            {
                try
                {
                    lp1.Parse();
                }
                catch (Exception ex)
                {
                    lp1.Display_Error(ex.Message);
                }
            }


            int k = 0;

            foreach (var item in lp1.HtmlElements_List)
            {
                Cmd_Render(item, k, builder);
            }



            base.BuildRenderTree(builder);
        }

    

        public void Cmd_Render(HtmlElement _item, int k, RenderTreeBuilder builder)
        {

            builder.OpenElement(k++, _item.Name);

            if (_item.attributes.Any())
            {
                foreach (var item in _item.attributes)
                {
                    builder.AddAttribute(k++, item.Key, item.Value.Replace("|","/"));
                }
            }

            if (!string.IsNullOrEmpty(_item.Content))
            {
                builder.AddContent(k++, _item.Content);
            }

        

            if (_item.children.Any())
            {

                foreach (HtmlElement item in _item.children)
                {
                    Cmd_Render(item, k, builder);
                }
            }

            builder.CloseElement();

        }


        public void Dispose()
        {

        }
    }
}
