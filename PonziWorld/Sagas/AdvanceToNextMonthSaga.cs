﻿using PonziWorld.Company;
using PonziWorld.DataRegion.InvestmentSummaryTabs.DepositorsTab;
using PonziWorld.DataRegion.InvestmentSummaryTabs.WithdrawersTab;
using PonziWorld.DataRegion.PerformanceHistoryTab;
using PonziWorld.DataRegion.TimeAdvancement;
using PonziWorld.Events;
using PonziWorld.Investments;
using PonziWorld.Investments.Investors;
using PonziWorld.MainWindow;
using Prism.Events;
using System.Collections.Generic;

namespace PonziWorld.Sagas;

internal class AdvanceToNextMonthSaga : SagaBase<AdvanceToNextMonthStartedEvent, AdvanceToNextMonthCompletedEvent>
{
    private Company.Company company = Company.Company.Default;
    private IEnumerable<Investor> allInvestors = new List<Investor>();
    private NewInvestmentsSummary newInvestmentsSummary = NewInvestmentsSummary.Default;
    private double claimedInterestRate = default;

    private bool hasAppliedCompanyInvestments = false;
    private bool hasRetrievedInvestors = false;

    private bool hasLoadedDeposits = false;
    private bool hasLoadedWithdrawals = false;
    private bool hasLoadedInvestors = false;
    private bool hasUpdatedCompanyFunds = false;
    private bool hasStoredPerformance = false;

    public AdvanceToNextMonthSaga(IEventAggregator eventAggregator)
        : base(eventAggregator)
    { }

    protected override void OnSagaStarted() =>
        StartProcess(AcquireClaimedInterest.Process, new(), OnClaimedInterestAcquired);

    private void OnClaimedInterestAcquired(ClaimedInterestAcquiredEventPayload incomingPayload)
    {
        claimedInterestRate = incomingPayload.ClaimedInterestRate;

        StartProcess(
            ApplyNewInterestRateToInvestors.Process,
            new(claimedInterestRate),
            OnNewInterestRateAppliedToInvestors);

        StartProcess(
            ApplyClaimedInterestRateToCompany.Process,
            new(claimedInterestRate),
            OnClaimedInterestRateAppliedToCompany);
    }

    private void OnNewInterestRateAppliedToInvestors(NewInterestRateAppliedToInvestorsEventPayload incomingPayload)
    {
        allInvestors = incomingPayload.AllInvestors;
        hasRetrievedInvestors = true;

        if (IsReadyToGenerateInvestments())
            GenerateInvestments();
    }

    private void OnClaimedInterestRateAppliedToCompany(ClaimedInterestRateAppliedToCompanyEventPayload incomingPayload)
    {
        company = incomingPayload.Company;

        StartProcess(
            DetermineCompanyInvestmentResults.Process,
            new(company),
            OnCompanyInvestmentResultsDetermined);

        StartProcess(
            StoreClaimedInterestRate.Process,
            new(company.Month, claimedInterestRate),
            OnClaimedInterestRateStored);
    }

    private void OnCompanyInvestmentResultsDetermined(
        CompanyInvestmentResultsDeterminedEventPayload incomingPayload) =>
        StartProcess(
            ApplyCompanyInvestmentResults.Process,
            new(incomingPayload.ProfitFromInvestments),
            OnCompanyInvestmentResultsDetermined);

    private void OnCompanyInvestmentResultsDetermined(CompanyInvestmentResultsAppliedEventPayload incomingPayload)
    {
        hasAppliedCompanyInvestments = true;

        if (IsReadyToGenerateInvestments())
            GenerateInvestments();
    }

    private bool IsReadyToGenerateInvestments() =>
        hasAppliedCompanyInvestments && hasRetrievedInvestors;

    private void GenerateInvestments() =>
        StartProcess(GenerateNewMonthInvestments.Process, new(company, allInvestors), OnNewMonthInvestmentsGenerated);

    private void OnNewMonthInvestmentsGenerated(NewMonthInvestmentsGeneratedEventPayload incomingPayload)
    {
        newInvestmentsSummary = incomingPayload.NewInvestmentsSummary;
        StartProcess(ApplyNewMonthInvestments.Process, new(newInvestmentsSummary), OnNewMonthInvestmentsApplied);
    }

    private void OnNewMonthInvestmentsApplied(NewMonthInvestmentsAppliedEventPayload incomingPayload)
    {
        StartProcess(UpdateCompanyFunds.Process, new(newInvestmentsSummary), OnCompanyFundsUpdated);
        StartProcess(LoadDepositsForNewMonth.Process, new(newInvestmentsSummary), OnNewMonthDepositsLoaded);
        StartProcess(LoadWithdrawalsForNewMonth.Process, new(newInvestmentsSummary), OnNewMonthWithdrawalsLoaded);
        StartProcess(LoadInvestors.Process, new(), OnInvestorsLoaded);
    }

    private void OnClaimedInterestRateStored(ClaimedInterestRateStoredEventPayload incomingPayload)
    {
        hasStoredPerformance = true;

        if (IsReadyToCompleteSaga())
            CompleteSaga();
    }

    private void OnCompanyFundsUpdated(CompanyFundsUpdatedEventPayload incomingPayload)
    {
        hasUpdatedCompanyFunds = true;

        if (IsReadyToCompleteSaga())
            CompleteSaga();
    }

    private void OnNewMonthDepositsLoaded(DepositsForNewMonthLoadedEventPayload incomingPayload)
    {
        hasLoadedDeposits = true;

        if (IsReadyToCompleteSaga())
            CompleteSaga();
    }

    private void OnNewMonthWithdrawalsLoaded(WithdrawalsForNewMonthLoadedEventPayload incomingPayload)
    {
        hasLoadedWithdrawals = true;

        if (IsReadyToCompleteSaga())
            CompleteSaga();
    }

    private void OnInvestorsLoaded(InvestorsLoadedEventPayload incomingPayload)
    {
        hasLoadedInvestors = true;

        if (IsReadyToCompleteSaga())
            CompleteSaga();
    }

    private bool IsReadyToCompleteSaga() =>
        hasStoredPerformance &&
        hasUpdatedCompanyFunds &&
        hasLoadedInvestors &&
        hasLoadedWithdrawals &&
        hasLoadedDeposits;

    protected override void ResetSaga()
    {
        company = Company.Company.Default;
        allInvestors = new List<Investor>();
        newInvestmentsSummary = NewInvestmentsSummary.Default;
        claimedInterestRate = default;

        hasAppliedCompanyInvestments = false;
        hasRetrievedInvestors = false;

        hasLoadedDeposits = false;
        hasLoadedWithdrawals = false;
        hasLoadedInvestors = false;
        hasUpdatedCompanyFunds = false;
        hasStoredPerformance = false;
    }
}