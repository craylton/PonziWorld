using PonziWorld.Events;
using PonziWorld.Investments;
using PonziWorld.Investments.Investors;
using Prism.Events;
using System.Collections.Generic;

namespace PonziWorld.Sagas;

internal class AdvanceToNextMonthSaga : SagaBase<AdvanceToNextMonthStartedEvent, AdvanceToNextMonthCompletedEvent>
{
    private Company.Company company = Company.Company.Default;
    private IEnumerable<Investor> allInvestors = new List<Investor>();
    private NewInvestmentsSummary newInvestmentsSummary = NewInvestmentsSummary.Default;

    private bool hasLoadedCompany = false;
    private bool hasRetrievedInvestors = false;

    private bool hasLoadedDeposits = false;
    private bool hasLoadedWithdrawals = false;
    private bool hasLoadedInvestors = false;
    private bool hasUpdatedCompanyFunds = false;

    public AdvanceToNextMonthSaga(IEventAggregator eventAggregator)
        : base(eventAggregator)
    { }

    // apply % to existing investors and update satisfaction
    // update company stats (suspicion etc)
    // calculate and store results of company's investments

    protected override void OnSagaStarted()
    {
        StartProcess(LoadCompany.Process, new(), OnCompanyLoaded);
        StartProcess(RetrieveInvestors.Process, new(), OnInvestorsRetrieved);
    }

    private void OnCompanyLoaded(CompanyLoadedEventPayload incomingPayload)
    {
        company = incomingPayload.Company;
        hasLoadedCompany = true;

        if (IsReadyToGenerateInvestments())
            GenerateInvestments();
    }

    private void OnInvestorsRetrieved(InvestorsRetrievedEventPayload incomingPayload)
    {
        allInvestors = incomingPayload.Investors;
        hasRetrievedInvestors = true;

        if (IsReadyToGenerateInvestments())
            GenerateInvestments();
    }

    private bool IsReadyToGenerateInvestments() =>
        hasLoadedCompany && hasRetrievedInvestors;

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
        hasUpdatedCompanyFunds && hasLoadedInvestors && hasLoadedWithdrawals && hasLoadedDeposits;

    protected override void ResetSaga()
    {
        company = Company.Company.Default;
        allInvestors = new List<Investor>();
        newInvestmentsSummary = NewInvestmentsSummary.Default;

        hasLoadedCompany = false;
        hasRetrievedInvestors = false;

        hasLoadedDeposits = false;
        hasLoadedWithdrawals = false;
        hasLoadedInvestors = false;
        hasUpdatedCompanyFunds = false;
    }
}
