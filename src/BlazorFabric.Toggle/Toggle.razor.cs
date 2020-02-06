﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class Toggle : FabricComponentBase
    {
        [CascadingParameter(Name = "Theme")]
        public ITheme Theme { get; set; }

        [Parameter] public bool? Checked { get; set; }
        [Parameter] public bool DefaultChecked { get; set; }
        [Parameter] public bool Disabled { get; set; }
        [Parameter] public bool InlineLabel { get; set; }
        [Parameter] public string Label { get; set; }
        [Parameter] public string OffText { get; set; }
        [Parameter] public string OnLabel { get; set; }

        [Parameter] public EventCallback<bool> CheckedChanged { get; set; }
        [Parameter] public string OnText { get; set; }

        protected bool IsChecked;
        protected string Id = Guid.NewGuid().ToString();
        protected string LabelId => Id + "-label";
        protected string StateTextId => Id + "-stateText";
        protected string BadAriaLabel;
        protected string LabelledById;
        protected string StateText => IsChecked ? OnText : OffText;

        private bool onOffMissing = false;

        private ICollection<Rule> ToggleRules { get; set; } = new List<Rule>();


        protected string GetClassExtras()
        {
            string classNames = "";

            if (InlineLabel)
                classNames += " ms-Toggle--inlineLabel";

            if (Disabled)
                classNames += " ms-Toggle--disabled";
            else
                classNames += " ms-Toggle--enabled";

            if (IsChecked)
                classNames += " ms-Toggle--checked";
            else
                classNames += " ms-Toggle--unchecked";

            if (onOffMissing)
                classNames += " ms-Toggle--onOffMissing";

            classNames += $" {ClassName}";

            return classNames;
        }

        protected override void OnInitialized()
        {
            CreateCss();
            IsChecked = this.Checked ?? this.DefaultChecked;
            base.OnInitialized();
        }

        protected override Task OnParametersSetAsync()
        {
            IsChecked = this.Checked ?? IsChecked;

            CreateCss();

            //if (IsChecked)
            //{
            //    StateText = OnText;
            //}
            //else
            //{
            //    StateText = OffText;
            //}
            if (string.IsNullOrWhiteSpace(AriaLabel) && string.IsNullOrWhiteSpace(BadAriaLabel))
                LabelledById = LabelId;
            else
                LabelledById = StateTextId;

            if (string.IsNullOrWhiteSpace(OffText) && string.IsNullOrWhiteSpace(OnText))
                onOffMissing = true;

            return base.OnParametersSetAsync();
        }

        protected Task OnClick(MouseEventArgs args)
        {
            Debug.WriteLine("Clicked");
            if (!Disabled)
            {
                Debug.WriteLine("Not Disabled");
                if (Checked == null)  // only update internally if Checked is not set
                {
                    Debug.WriteLine($"Checked not set so switch to: {!IsChecked}");
                    IsChecked = !IsChecked;
                }
            }

            return this.CheckedChanged.InvokeAsync(!IsChecked);

            //return Task.CompletedTask;
        }

        private void CreateCss()
        {
            ToggleRules.Clear();
            ToggleRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Toggle" }, Properties = new CssString() { Css = $"margin-bottom:8px;" } });
            ToggleRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Toggle--inlineLabel" }, Properties = new CssString() { Css = "display:flex;align-items:center;" } });
            ToggleRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Toggle--disabled .ms-Toggle-label" }, Properties = new CssString() { Css = $"color:{Theme.SemanticTextColors.DisabledText};" } });
            ToggleRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Toggle--inlineLabel .ms-Toggle-label" }, Properties = new CssString() { Css = $"margin-right:16px;" } });
            ToggleRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Toggle--inlineLabel.ms-Toggle--onOffMissing .ms-Toggle-label" }, Properties = new CssString() { Css = "margin-right:initial;margin-left:16px;order:1;" } });
            ToggleRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Toggle-container" }, Properties = new CssString() { Css = "display:inline-flex;position:relative;" } });
            ToggleRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Toggle-pill" }, Properties = new CssString() { Css = $"outline:transparent;position:relative;font-size:20px;box-sizing:border-box;width:40px;height:20px;border-radius:10px;transition:all 0.1s ease;border:1px solid {Theme.SemanticColors.SmallInputBorder};background:{Theme.SemanticColors.BodyBackground};cursor:pointer;display:flex;align-items:center;padding:0 3px;" } });
            ToggleRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Toggle-pill::-moz-focus-inner" }, Properties = new CssString() { Css = $"border:0;" } });
            ToggleRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Fabric--isFocusVisible .ms-Toggle-pill:focus:after" }, Properties = new CssString() { Css = $"content:'';position:absolute;left:2px;top:2px;bottom:2px;right:2px;border:1px solid {Theme.Palette.White}; outline:1px solid {Theme.Palette.NeutralSecondary}; z-index:var(--zindex-FocusStyle);" } });
            ToggleRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Toggle--enabled.ms-Toggle--unchecked .ms-Toggle-pill:hover" }, Properties = new CssString() { Css = $" border-color:{Theme.SemanticColors.InputBorderHovered};" } });
            ToggleRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Toggle--enabled.ms-Toggle--unchecked .ms-Toggle-pill:hover .ms-Toggle-thumb" }, Properties = new CssString() { Css = $"background-color:{Theme.Palette.NeutralDark};" } });
            ToggleRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Toggle--enabled.ms-Toggle--checked .ms-Toggle-pill" }, Properties = new CssString() { Css = $"background:{Theme.SemanticColors.InputBackgroundChecked};border-color:transparent;justify-content:flex-end;" } });
            ToggleRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Toggle--enabled.ms-Toggle--checked .ms-Toggle-pill:hover" }, Properties = new CssString() { Css = $"background-color:{Theme.Palette.ThemeDark};border-color:transparent;" } });
            ToggleRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Toggle--disabled .ms-Toggle-pill" }, Properties = new CssString() { Css = $"cursor:default;" } });
            ToggleRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Toggle--disabled.ms-Toggle--unchecked .ms-Toggle-pill" }, Properties = new CssString() { Css = $"border-color:{Theme.SemanticTextColors.DisabledBodySubtext};" } });
            ToggleRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Toggle--disabled.ms-Toggle--checked .ms-Toggle-pill" }, Properties = new CssString() { Css = $"background-color:{Theme.SemanticTextColors.DisabledBodySubtext};border-color:transparent;justify-content:flex-end;" } });
            ToggleRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Toggle-thumb" }, Properties = new CssString() { Css = $"width:12px;height:12px;border-radius:50%;transition:all 0.1s ease;background-color:{Theme.SemanticColors.SmallInputBorder};border-color:transparent;border-width:.28em;border-style:solid;box-sizing:border-box;" } });
            ToggleRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Toggle--enabled.ms-Toggle--checked .ms-Toggle-thumb" }, Properties = new CssString() { Css = $"background-color:{Theme.SemanticColors.InputForegroundChecked};" } });
            ToggleRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Toggle--disabled.ms-Toggle--unchecked .ms-Toggle-thumb" }, Properties = new CssString() { Css = $"background-color:{Theme.SemanticTextColors.DisabledBodySubtext};" } });
            ToggleRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Toggle--disabled.ms-Toggle--checked .ms-Toggle-thumb" }, Properties = new CssString() { Css = $"background-color:{Theme.SemanticColors.DisabledBackground};" } });
            ToggleRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Label.ms-Toggle-stateText" }, Properties = new CssString() { Css = $"padding:0;margin:0 8px;user-select:none;font-weight:{Theme.FontStyle.FontWeight.Regular};" } });
            ToggleRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Toggle--disabled .ms-Label.ms-Toggle-stateText" }, Properties = new CssString() { Css = $"color:{Theme.SemanticTextColors.DisabledText};" } });
            ToggleRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" }, Properties = new CssString() { Css = ".ms-Toggle--disabled .ms-Toggle-label{color:GrayText;}.ms-Toggle--enabled.ms-Toggle--unchecked .ms-Toggle-pill:hover.ms-Toggle-thumb{border-color:Highlight;}.ms-Toggle--enabled.ms-Toggle--checked .ms-Toggle-pill:hover{background-color:Highlight;} .ms-Toggle--enabled.ms-Toggle--checked .ms-Toggle-pill{background-color:WindowText;} .ms-Toggle--enabled.ms-Toggle-pill:hover{border-color:Highlight;} .ms-Toggle--enabled.ms-Toggle--checked .ms-Toggle-thumb{background-color: Window;border-color:Window;} .ms-Toggle--disabled.ms-Toggle-stateText{color:GrayText;}" } });
        }
    }
}
