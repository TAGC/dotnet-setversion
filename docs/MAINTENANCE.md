# Maintainer Standard Operating Procedure (SOP)

This document is for the repository owner. It covers internal maintenance tasks and the release process using GitKraken GitFlow.

---

## The Release Ceremony

Follow these steps to release a new version of `dotnet-setversion`.

### 1. Maintain the Version on Develop
Whenever changes are merged into `develop`, determine if a version bump is required:
- **First merge since last release:** Always required.
- **Escalation:** Bump from Patch to Minor if new features were added, or to Major if breaking changes (like dropping .NET versions) were introduced.
- **Action:** Update `GitVersion.yml` on the `develop` branch immediately after merging.
- **Commit:** Commit and push this change.

### 2. Start the Release
- Open GitKraken and ensure you are on the updated `develop` branch.
- Click the **GitFlow** button and select **Start Release**.
- Enter the version number (e.g., `4.0.0`). 
- *Note: Since GitVersion.yml is already updated, this branch is ready for final verification.*

### 3. Finish the Release
- In GitKraken, click the **GitFlow** button and select **Finish Release**.
- Ensure the following are checked:
  - Merge into master
  - Merge into develop
  - Tag the release (e.g., `v4.0.0`)
- Click **Finish Release**.

### 4. Push to GitHub
GitKraken performs merges and tagging locally. You must manually push the updates:
- Push the `master` branch.
- Push the `develop` branch.
- Push the new **Tag**.

---

## NuGet API Key Rotation

If an AppVeyor deployment fails with a `401 Unauthorized` or `403 Forbidden` error, the NuGet API key has likely expired. This typically needs to be done once a year.

### 1. Regenerate the Key
1. Log in to [NuGet.org](https://www.nuget.org/account/apikeys).
2. Find the key for `dotnet-setversion` and click **Regenerate**.
3. Copy the new key immediately.

### 2. Secure the Key
1. Update the entry in **1Password** with the new plain-text key.

### 3. Encrypt for AppVeyor
1. Go to the [AppVeyor Encryption Tool](https://ci.appveyor.com/tools/encrypt).
2. Paste the plain-text API key and click **Encrypt**.
3. Copy the resulting `secure: <string>` value.

### 4. Update Configuration
1. Open `appveyor.yml` in the root of the repo.
2. Replace the old `secure` string in the `deploy:` section with the new one.
3. Commit and push: `build: update encrypted NuGet API key`.