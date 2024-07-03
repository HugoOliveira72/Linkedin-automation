﻿using forms.Models.Filters;
using forms.Models.Interfaces;
using forms.Models.PageObjects;
using forms.Models.PageObjects.Base;
using forms.Models.PageObjects.Sections;
using forms.Presenters.Services;
using forms.Repositories;
using forms.Utilities.Messages;
using forms.Views.Interfaces;
using Linkedin_Automation.Config;
using Linkedin_Automation.Model;
using Microsoft.Playwright;

namespace forms.Presenters
{
    public class HomePresenter
    {
        private IHomeView _homeView;
        private IDataService<dynamic> _dataService;
        private ILogService _logService;
        private ILoginRepository _loginRepository;
        private ILogRepository _logRepository;
        private OutputStringPatterns stringPatterns = new();
        private CancellationTokenSource cancellationToken = new();

        private PlaywrightConfiguration _settings;
        private int _appliedJobs;
        private int _savedJobs;
        private BasePage _basePage;

        public HomePresenter(
            IHomeView homeView,
            IDataService<dynamic> dataService,
            ILogService logService,
            ILoginRepository loginRepository,
            ILogRepository logRepository)
        {
            _homeView = homeView;
            _dataService = dataService;
            _logService = logService;
            _homeView.StartAutomation += StartAutomation;
            _homeView.StopAutomation += StopAutomation;
            _loginRepository = loginRepository;
            _logRepository = logRepository;
        }

        private async void StartAutomation(object sender, EventArgs e)
        {
            ///Desativar botão play
            _homeView.ButtonPlayEnabled = false;
            ///Habilitar botão stop
            _homeView.ButtonStopEnabled = true;
            await Script(cancellationToken.Token);
        }

        private void StopAutomation(object sender, EventArgs e)
        {
            cancellationToken.Cancel();
        }

        //Automation Method
        private async Task Script(CancellationToken token)
        {
            try
            {
                // OBTEM DADOS DOS FILTROS
                FilterFieldsModel filterData = _dataService.GetData();

                // VERIFICAÇÃO EXISTENCIA LOG.TXT
                _logService.LogFileExistingVerification();

                // CONFIGURAÇÃO DO PLAYWRIGHT
                IConfigRepository configRepository = new ConfigRepository();
                PlaywrightConfiguration playwrightConfiguration = new PlaywrightConfiguration(configRepository);
                _settings = await playwrightConfiguration.launchSettingsAsync();

                //INICIO
                IPage page = await _settings.BrowserContext!.NewPageAsync();
                _basePage = new BasePage(page, token);

                await AddMessageToRichTextbox(stringPatterns.linePattern());
                await AddMessageToRichTextbox("Iniciando automação\n");
                await AddMessageToRichTextbox("Abrindo o navegador padrão...\n");

                // DIRECIONANDO
                await AddMessageToRichTextbox("Direcionando para https://www.linkedin.com/\n");
                await Task.Delay(TimeSpan.FromSeconds(0.5));

                await page.GotoAsync("https://www.linkedin.com/");
                await Task.Delay(TimeSpan.FromSeconds(5));

                await page.GetByLabel("Principal").GetByRole(AriaRole.Link, new() { Name = "Entrar" }).ClickAsync();
                await Task.Delay(TimeSpan.FromSeconds(1));

                #region LoginPage
                /// LER DADOS DE USUARIO
                UserModel userInfo = _loginRepository.ReadAndConvertMsgpackFileToObject();

                /// PREENCHIMENTO DAS CREEDENCIAIS
                await AddMessageToRichTextbox("Fazendo login..\n");

                ///Login page
                LoginPage loginPage = await LoginPage.BuildAsync(page, token);
                await loginPage.LoginAsync(userInfo.email, userInfo.password);

                //if (await loginPage.HandleErrorLoginAsync())
                //    return;

                await AddMessageToRichTextbox("Login bem sucedido\n");
                await Task.Delay(TimeSpan.FromSeconds(2));

                #endregion

                #region Securityhandle
                // CÓDIGO LINKEDIN / VERIFICAÇÃO DE SEGURANÇA (MANUALMENTE)
                await AddMessageToRichTextbox("Carregando...\n");
                await AddMessageToRichTextbox("Verifação de segurança\n");
                var message = await _basePage.WaitForElementAndHandleExceptionAsync(page, "#global-nav-typeahead", "Página carregada!", ExceptionMessages.SecurityError);
                await AddMessageToRichTextbox(message);
                await Task.Delay(TimeSpan.FromSeconds(2));

                #endregion

                #region JobPage Search Section
                // PESQUISA DE VAGAS
                await AddMessageToRichTextbox(stringPatterns.linePattern());
                await AddMessageToRichTextbox($"Pesquisando a vaga {_homeView.Job}\n");

                FeedPage feedPage = await FeedPage.BuildAsync(page, token);
                await feedPage._jobSpan!.ClickAsync();

                JobPage jobPage = await JobPage.BuildAsync(page, token);
                await jobPage.SearchJobAsync(_homeView.Job);

                await AddMessageToRichTextbox($"Pesquisado {_homeView.Job} com sucesso\n");
                await Task.Delay(TimeSpan.FromSeconds(2));

                #endregion

                #region FilterSection
                await AddMessageToRichTextbox(stringPatterns.linePattern());
                if (filterData != null)
                {
                    FilterSection jobSearchPage = await FilterSection.BuildAsync(page, token);

                    await AddMessageToRichTextbox("Aplicando filtros\n");
                    await jobSearchPage.GoToFilterSection(0.8);

                    ///Classificar por
                    await AddMessageToRichTextbox(FilterLabelsModel.ClassifyByLabel);
                    await jobSearchPage.SelectFilter(filterData.ClassifyBy);
                    ///Data do anúncio
                    await AddMessageToRichTextbox(FilterLabelsModel.AnnouncimentDateLabel);
                    await jobSearchPage.SelectFilter(filterData.AnnoucementDate);
                    ///Nível de experiência
                    await AddMessageToRichTextbox(FilterLabelsModel.ExperienceLevelLabel);
                    await jobSearchPage.SelectFilter(FilterLabelsModel.ExperienceLevelLabel, filterData.CheckedListBoxExperiences);
                    ///Tipo de vaga
                    await AddMessageToRichTextbox(FilterLabelsModel.JobTypeLabel);
                    await jobSearchPage.SelectFilter(FilterLabelsModel.JobTypeLabel, filterData.CheckedListBoxType_job);
                    ///Remoto
                    await AddMessageToRichTextbox(FilterLabelsModel.RemoteTypeLabel);
                    await jobSearchPage.SelectFilter(FilterLabelsModel.RemoteTypeLabel, filterData.CheckedListBoxRemote);

                    ///Apply All filters
                    await jobSearchPage.ApplyFilter();

                }
                else
                {
                    await AddMessageToRichTextbox("Não há filtros para serem aplicados!\n");
                }
                await Task.Delay(TimeSpan.FromSeconds(3));

                #endregion

                //JOB LIST SECTION
                #region GetAllJobs Elements
                /*
                    - avaiableJobs, Vagas disponiveis podem ser candidadatas
                    - appliedJobs, Vagas candidatadas
                    - savedJobs, Vagas que contem perguntas, são salvas para preenchimento manual posterior
                */
                int currentPage = 1, jobsCounter = 0;
                _appliedJobs = 0;
                _savedJobs = 0;

                // Instancia jobListSection
                await AddMessageToRichTextbox(stringPatterns.linePattern());

                //Quando não encontra nenhuma vaga
                ILocator? noFoundJob = page.GetByText("Nenhuma vaga corresponde aos seus critérios.");
                if (noFoundJob != null)
                {
                    await AddMessageToRichTextbox(ExceptionMessages.CouldNotFoundTheJob);
                    await AddMessageToRichTextbox("Buscando sugestões...");
                    await AddMessageToRichTextbox(stringPatterns.linePattern());
                }

                JobListSection jobListSection = await JobListSection.BuildAsync(page, _homeView, token, currentPage);
                int avaiableJobs = jobListSection.getAvailableJob();

                if (avaiableJobs > 0)
                {
                    await AddMessageToRichTextbox($"Quantidade de vagas encontradas: {avaiableJobs}");
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
                else
                {
                    await AddMessageToRichTextbox(ExceptionMessages.CouldNotFoundTheJob);
                    await CloseBrowserAndHandleButtonsVisibily(_settings, _appliedJobs, _savedJobs);
                    return;
                }

                #endregion

                //APLICAÇÃO DE VAGAS
                while (_appliedJobs != Int32.Parse(_homeView.AmountJobs))
                {
                    jobsCounter++;

                    //JOB LIST SECTION
                    #region NextPageValidation
                    if (jobsCounter > avaiableJobs)
                    {
                        ///Possui próxima página
                        bool hasNextPage = await jobListSection.GoToNextPage();
                        // Fechar aplicação
                        if (!hasNextPage)
                        {
                            await CloseBrowserAndHandleButtonsVisibily(_settings, _appliedJobs, _savedJobs);
                            return;
                        }
                        //Recarregar elementos
                        else
                        {
                            await jobListSection.ReloadUlElements();
                            avaiableJobs = jobListSection.getAvailableJob();
                            currentPage++;
                            jobsCounter = 1;
                            await AddMessageToRichTextbox(stringPatterns.ShowFinalResult(_appliedJobs, _savedJobs));
                            await AddMessageToRichTextbox(stringPatterns.linePattern());
                            await AddMessageToRichTextbox($"Vagas encontradas: {avaiableJobs}");
                        }
                    }

                    #endregion

                    await AddMessageToRichTextbox(stringPatterns.linePattern());
                    await AddMessageToRichTextbox($"Percorrendo Página {currentPage}");
                    await AddMessageToRichTextbox($"Selecionando Vaga nº {jobsCounter}");

                    #region SelectJob
                    try
                    {
                        await jobListSection.ClickOnJob(jobsCounter);
                    }
                    catch (Exception e)
                    {
                        await AddMessageToRichTextbox($"\n\n{stringPatterns.linePattern}{e}");
                        _logRepository.WriteALogError(ExceptionMessages.ErrorToSelectJob, e);
                    }

                    #endregion

                    //DETAIL SECTION
                    #region Handle subscribe button
                    JobDetailsSection jobDetailsSection = await JobDetailsSection.BuildAsync(page, token);
                    // BOTÃO (Candidatar-se a vaga)
                    if (jobDetailsSection._subscribeButton == null)
                    {
                        // VERIFICAR SE VAGA ESTÁ INCOMPLETA (Continue)
                        if (jobDetailsSection._continueButton != null)
                        {
                            // SALVAR VAGA
                            if (jobDetailsSection._jobAlreadySaved != null)
                            {
                                await AddMessageToRichTextbox("VAGA JÁ FOI SALVA!");
                            }
                            else
                            {
                                await jobDetailsSection._saveButton!.ClickAsync();
                                _savedJobs = SetAndCountSavedJobs(_savedJobs);
                                await ShowSavedJobsMessage(jobsCounter, _savedJobs);
                            }
                        }
                        else if (await jobDetailsSection.CheckSubscribedStatus())
                        {
                            // VERIFICAR SE VAGA JA FOI INSCRITA
                            await AddMessageToRichTextbox("!Vaga já candidatada!");
                        }
                        continue;
                    }
                    else
                    {
                        // SE INSCREVE NA VAGA
                        await jobDetailsSection._subscribeButton!.ClickAsync();
                    }

                    #endregion

                    //POPUP WINDOW SECTION
                    #region Advance button
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    PopupWindowSection popupWindowSection = await PopupWindowSection.BuildAsync(page, token);
                    if (popupWindowSection._advanceButton == null)// QUANDO BOTÃO AVANÇAR Ñ EXISTE
                    {
                        /// CLICA BOTÃO ENVIAR CANDIDATURA
                        await popupWindowSection.SendJobApplicationAndClosePage();

                        /// EXIBE NA TELA INFORMAÇÕES DA CANDIDATURA
                        _appliedJobs = await SetAndCountAppliedJobsAndShow(_appliedJobs, jobsCounter);
                        continue;
                    }
                    else // QUANDO BOTÃO AVANÇAR EXISTE!
                    {
                        try
                        {
                            /// CLICA NO BOTÃO AVANÇAR
                            await popupWindowSection._advanceButton!.ClickAsync();
                        }
                        catch (Exception e)
                        {
                            /// ERRO AO CLICAR NO BOTÃO
                            _logRepository.WriteALogError(ExceptionMessages.CouldNotClickedTheAdvanceButton, e);
                            continue;
                        }
                    }

                    #endregion

                    #region Review button
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    popupWindowSection._reviewButton = await popupWindowSection.LoadElementAsync("span:has-text('Revisar')");
                    popupWindowSection._advanceButton = await popupWindowSection.LoadElementAsync("button[aria-label='Avançar para próxima etapa']");
                    if (popupWindowSection._reviewButton == null) // QUANDO BOTÃO REVISAR Ñ EXISTE
                        await popupWindowSection._advanceButton.ClickAsync(); /// Haverá o botão avançar
                    else// QUANDO BOTÃO REVISAR EXISTE
                    {
                        try
                        {
                            await popupWindowSection._reviewButton!.ClickAsync();
                        }
                        catch (Exception e)
                        {
                            await Task.Delay(TimeSpan.FromSeconds(1));
                            _logRepository.WriteALogError(ExceptionMessages.CouldNotClickedTheAdvanceButton, e);
                        }
                    }

                    #endregion

                    #region Addicional Questions

                    popupWindowSection._additionalQuestions = await popupWindowSection.LoadElementAsync("h3");
                    popupWindowSection._advanceButton = await popupWindowSection.LoadElementAsync("button[aria-label='Avançar para próxima etapa']");
                    popupWindowSection._reviewButton = await popupWindowSection.LoadElementAsync("span:has-text('Revisar')");

                    if (!await popupWindowSection.CheckAddicionalQuestions()) // QUANDO NÃO HÁ PERGUNTAS
                    {
                        if (popupWindowSection._reviewButton != null) ///Quando possui o botão review
                        {
                            await popupWindowSection._reviewButton.ClickAsync();
                            await Task.Delay(TimeSpan.FromSeconds(0.8));
                        }
                        // ENVIAR CANDIDATURA, SEM PERGUNTAS
                        await popupWindowSection.SendJobApplicationAndClosePage();

                        _appliedJobs = await SetAndCountAppliedJobsAndShow(_appliedJobs, jobsCounter);
                        await AddMessageToRichTextbox(stringPatterns.ShowFinalResult(_appliedJobs, _savedJobs));
                        continue;
                    }
                    else if (popupWindowSection._additionalQuestions != null) // QUANDO HÁ PERGUNTAS
                    {
                        await popupWindowSection.SaveJobClosePage();
                        _savedJobs = SetAndCountSavedJobs(_savedJobs);
                        await Task.Delay(TimeSpan.FromSeconds(0.8));
                        await AddMessageToRichTextbox($"Salva a vaga nº{jobsCounter}");
                        await Task.Delay(TimeSpan.FromSeconds(0.8));
                        await AddMessageToRichTextbox(stringPatterns.ShowFinalResult(_appliedJobs, _savedJobs));
                        continue;
                    }
                    else if (popupWindowSection._advanceButton != null) // BOTÃO AVANÇAR
                    {
                        await popupWindowSection._advanceButton.ClickAsync();
                    }

                    #endregion
                }
            }
            catch (OperationCanceledException)
            {
                await CloseBrowserAndHandleButtonsVisibily(_settings, _appliedJobs, _savedJobs);
                return;
            }
            catch (Exception e)
            {
                _logRepository.WriteALogError(ExceptionMessages.CommonError, e);
                return;
            }

        }

        //UI
        private async Task ShowAppliedJobsMessage(int jobsCounter, int appliedJobs)
        {
            await AddMessageToRichTextbox($"Inscrito na vaga nº{jobsCounter}");
            await AddMessageToRichTextbox($"Total de {appliedJobs} vagas aplicadas");
        }

        private async Task ShowSavedJobsMessage(int jobsCounter, int savedJobs)
        {
            await AddMessageToRichTextbox($"Vaga nº{jobsCounter} Salva");
            await AddMessageToRichTextbox($"Total de {savedJobs} vagas salvas");
        }

        private async Task AddMessageToRichTextbox(string message, bool verifyToken = true)
        {
            if (verifyToken) _basePage.VerifyToken();
            _homeView.RichtxtBox += $"{message}\n";
            await Task.Delay(TimeSpan.FromSeconds(0.1));
        }

        //LOGIC/UI
        private async Task<int> SetAndCountAppliedJobsAndShow(int amountOfAppliedJobs, int counterOfJobs)
        {
            amountOfAppliedJobs++;
            _homeView.AmountOfAppliedJobs = amountOfAppliedJobs;
            await ShowAppliedJobsMessage(counterOfJobs, amountOfAppliedJobs);
            return amountOfAppliedJobs;
        }

        private int SetAndCountSavedJobs(int amountOfsavedJobs)
        {
            amountOfsavedJobs++;
            _homeView.AmountOfSavedJobs = amountOfsavedJobs;
            return amountOfsavedJobs;
        }

        private async Task CloseBrowserAndHandleButtonsVisibily(PlaywrightConfiguration playwrightConfiguration, int appliedJobsAmount, int savedJobsAmount)
        {
            ///Fechar o navegador
            await AddMessageToRichTextbox(stringPatterns.linePattern(), false);
            await AddMessageToRichTextbox("Fechando navegador...", false);
            await AddMessageToRichTextbox("AUTOMAÇÃO FINALIZADA", false);
            await AddMessageToRichTextbox(stringPatterns.ShowFinalResult(appliedJobsAmount, savedJobsAmount), false);
            await playwrightConfiguration.BrowserContext.CloseAsync();
            ///Ativar o botão play
            _homeView.ButtonPlayEnabled = true;
            ///Desativar o botão Stop
            _homeView.ButtonStopEnabled = false;
        }
    }
}
