﻿using forms.Models.Interfaces;
using forms.Models.PageObjects.Base;
using forms.Repositories;
using forms.Utilities.Messages;
using Microsoft.Playwright;

namespace forms.Models.PageObjects.Sections
{
    public class PopupWindowSection : BasePage
    {
        private IPage _page;
        public IElementHandle? _advanceButton;
        public IElementHandle? _sendJobApplicationButton;
        public IElementHandle? _closeButton;
        public IElementHandle? _reviewButton;
        public IElementHandle? _saveButton;
        public IElementHandle? _additionalQuestions;
        public IElementHandle? _workExperienceElement;

        private ILogRepository _logRepository;

        public PopupWindowSection(IPage page, ILogRepository logRepository, CancellationToken token) : base(page, token)
        {
            _page = page;
            _logRepository = logRepository;
        }

        public static async Task<PopupWindowSection> BuildAsync(IPage page, ILogRepository logRepository, CancellationToken token, double securityTime = 0.5)
        {
            PopupWindowSection obj = new PopupWindowSection(page, logRepository, token);
            await obj.InicializateAsync(securityTime);
            return obj;
        }

        private async Task InicializateAsync(double securityTime)
        {
            await Task.Delay(TimeSpan.FromSeconds(securityTime));
            _advanceButton = await LoadElementAsync("button[aria-label='Avançar para próxima etapa']");
            await Task.Delay(TimeSpan.FromSeconds(securityTime));
        }

        public async Task<bool> CheckAddicionalQuestions()
        {
            return new[] { "Addicional", "Perguntas adicionais", "Additional Questions"}.Any(obj => _additionalQuestions!.ToString()!.Contains(obj));
        }

        public async Task SendJobApplicationAndClosePage(double securityTime = 0.5)
        {
            //Load elements
            _sendJobApplicationButton = await LoadElementAsync("button:has-text('Enviar candidatura')");
            //Click elements
            await Task.Delay(TimeSpan.FromSeconds(securityTime));
            if (_sendJobApplicationButton != null)
                await _sendJobApplicationButton.ClickAsync();
            await Task.Delay(TimeSpan.FromSeconds(1));
            await _page.Keyboard.PressAsync("Escape");
        }

        public async Task SaveJobClosePage(double securityTime = 0.5)
        {
            //Load elements
            _closeButton = await LoadElementAsync("button[aria-label='Fechar']");
            await Task.Delay(TimeSpan.FromSeconds(securityTime));
            //Click Elements
            await _closeButton.ClickAsync();
            _saveButton = await LoadElementAsync("button[data-control-name='save_application_btn']");
            await Task.Delay(TimeSpan.FromSeconds(securityTime));
            await _saveButton.ClickAsync();
        }

        public async Task CheckSecurityReminder()
        {
            if (await _page.GetByLabel("Lembrete de segurança da").GetByText("Antes de se candidatar,").IsVisibleAsync())
                await _page.GetByText("Continuar candidatura").ClickAsync();
        }
    }
}
