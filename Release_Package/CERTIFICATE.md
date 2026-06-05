# Certificate Installation (Optional)

This package includes a self-signed certificate for the CurrencyApp.

## Do I Need to Install the Certificate?

Usually **NO** - Windows 10/11 handles this automatically.

Only install manually if you get certificate errors during installation.

## How to Install Certificate

### Method 1: Automatic (Recommended)
1. Double-click CurrencyApp_Certificate.pfx
2. Click "Next"
3. Leave password empty (or enter: password123)
4. Select "Place all certificates in the Trusted Root Certification Authorities"
5. Click "Finish"

### Method 2: PowerShell (Admin)
\\\powershell
Import-PfxCertificate -FilePath "CurrencyApp_Certificate.pfx" -CertStoreLocation "Cert:\LocalMachine\Root" -Password (ConvertTo-SecureString "password123" -AsPlainText -Force)
\\\

---
This certificate is self-signed and safe to install locally.
