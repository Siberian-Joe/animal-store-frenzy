using System;
using System.Text;
using Game.SceneNavigation.Routes;
using Game.World.Inventory;
using Game.World.PlayerInteraction;
using Game.World.Quests.Progress;
using Game.World.Store;
using Modules.Presentation.Runtime.Panels;
using Modules.SceneNavigation.Runtime.Contracts;
using R3;
using UnityEngine;

namespace Game.Presentation.Screens.Core
{
    public sealed class CoreScreenPresenter : PanelPresenter<CoreScreen>
    {
        private readonly ISceneNavigator _sceneNavigator;
        private readonly IQuestProgressReader _questProgressReader;
        private readonly IPlayerInventoryReader _playerInventoryReader;
        private readonly IPlayerFeedbackReader _feedbackReader;
        private readonly IPlayerFeedback _feedback;
        private readonly IStoreRuntimeResolver _storeResolver;
        private readonly CompositeDisposable _disposables = new();

        public CoreScreenPresenter(
            ISceneNavigator sceneNavigator,
            IQuestProgressReader questProgressReader,
            IPlayerInventoryReader playerInventoryReader,
            IPlayerFeedbackReader feedbackReader,
            IPlayerFeedback feedback,
            IStoreRuntimeResolver storeResolver)
        {
            _sceneNavigator = sceneNavigator ?? throw new ArgumentNullException(nameof(sceneNavigator));
            _questProgressReader = questProgressReader ?? throw new ArgumentNullException(nameof(questProgressReader));
            _playerInventoryReader =
                playerInventoryReader ?? throw new ArgumentNullException(nameof(playerInventoryReader));
            _feedbackReader = feedbackReader ?? throw new ArgumentNullException(nameof(feedbackReader));
            _feedback = feedback ?? throw new ArgumentNullException(nameof(feedback));
            _storeResolver = storeResolver ?? throw new ArgumentNullException(nameof(storeResolver));
        }

        protected override void OnPanelAttached(CoreScreen panel)
        {
            panel.MainMenuRequested
                .Subscribe(_ => ReturnToMainMenu())
                .AddTo(_disposables);

            _questProgressReader.ProgressChanged
                .Subscribe(snapshot => RenderQuest(snapshot))
                .AddTo(_disposables);

            _questProgressReader.QuestCompleted
                .Subscribe(_ => _feedback.ShowMessage("Objective completed"))
                .AddTo(_disposables);

            _playerInventoryReader.Changed
                .Subscribe(_ => RenderInventory())
                .AddTo(_disposables);

            _feedbackReader.MessageShown
                .Subscribe(panel.SetFeedbackText)
                .AddTo(_disposables);

            RenderQuest(_questProgressReader.Current);
            RenderInventory();
            panel.SetFeedbackText(_feedbackReader.CurrentMessage);
        }

        protected override void OnOpened()
        {
            if (Panel == false)
                return;

            RenderQuest(_questProgressReader.Current);
            RenderInventory();
            Panel.SetFeedbackText(_feedbackReader.CurrentMessage);
        }

        protected override void OnReleased() => _disposables.Dispose();

        private void RenderQuest(QuestProgressSnapshot snapshot)
        {
            if (Panel == false)
                return;

            if (snapshot.Quests == null || snapshot.Quests.Count == 0)
            {
                Panel.SetObjectiveText(string.Empty);
                return;
            }

            var builder = new StringBuilder();
            var quest = snapshot.Quests[0];
            builder.AppendLine(quest.Title);

            if (string.IsNullOrWhiteSpace(quest.Summary) == false)
                builder.AppendLine(quest.Summary);

            foreach (var objective in quest.Objectives)
            {
                builder
                    .Append(objective.IsCompleted ? "[x] " : "[ ] ")
                    .AppendLine(objective.Text);
            }

            AppendShift(builder);
            Panel.SetObjectiveText(builder.ToString().TrimEnd());
        }

        private void AppendShift(StringBuilder builder)
        {
            if (_storeResolver.TryGetAnyShift(out var shift) == false)
                return;

            builder
                .AppendLine()
                .Append("Shift: ")
                .AppendLine(shift.Status.ToString())
                .Append("Customers: ")
                .Append(shift.ServedCustomers)
                .Append(" / ")
                .AppendLine(shift.RequiredCustomers.ToString())
                .Append("Revenue: ")
                .Append(shift.Revenue);
        }

        private void RenderInventory()
        {
            if (Panel == false)
                return;

            var inventory = _playerInventoryReader.Inventory;
            if (inventory.Items.Count == 0)
            {
                Panel.SetInventoryText("Inventory: (empty)");
                return;
            }

            var builder = new StringBuilder();
            builder.AppendLine("Inventory:");

            foreach (var item in inventory.Items)
                builder.AppendLine($"{item.ItemId}: {item.Amount}");

            Panel.SetInventoryText(builder.ToString().TrimEnd());
        }

        private void ReturnToMainMenu()
        {
            if (_sceneNavigator.TryGoTo<MainMenuRoute>() == false)
                Debug.LogWarning($"{nameof(ISceneNavigator)} rejected transition to {nameof(MainMenuRoute)}. " +
                                 "Transition is probably already running.");
        }
    }
}