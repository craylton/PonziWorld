# PonziWorld (archived)

Single?player desktop simulation/management game where you run a fictional investment company with, let's say, suspicious accounting. Advance time month by month, attract investors, handle deposits/withdrawals, and watch performance history evolve.

Status: superseded by a multiplayer web version
- New project: https://github.com/craylton/PonziWorld2
- If you are evaluating this repository, check the web version first.

High?level overview
- WPF app targeting .NET 8
- Data stored in MongoDB (see `PonziWorld/App.config` for the connection string)
- Prism (Unity) for DI, events, and simple sagas
- MahApps.Metro for dialogs and theming, Serilog for logging
- Two projects:
  - `PonziWorld`: the WPF application
  - `NameGenerator`: utility for generating random investor names

Core concepts
- Company (`Company.Company`): tracks month, claimed/actual funds, attractiveness, fame, and suspicion.
- Investors (`Investments.Investors.Investor`): have total funds, current investment, and satisfaction; may reinvest or withdraw.
- Time advancement (`MainTabs/TimeAdvancement`): generates prospective investors, new investments, reinvestments, and withdrawals for the next month, then applies them to the DB.
- Persistence: repositories (`MongoDb*Repository`) use `MongoDbRepositoryBase` to talk to MongoDB.

Run locally
1. Prereqs: .NET 8 SDK, MongoDB reachable (local default is `mongodb://localhost:27017`).
2. Configure DB: update `PonziWorld/App.config` connection string if needed.
3. Build and run `PonziWorld`. On startup the splash screen tests DB connectivity and enables actions accordingly.

Notes
- Logs are written to `logs/` via Serilog.
- UI features include tabs for Time Advancement, Monthly Investments, Performance History, and Investor details.

Again, this codebase is kept for reference; active development continues in the web version: https://github.com/craylton/PonziWorld2
