/*
# -------------------------------------------------------------------------------
# Author:      Cody Martin <cody.martin@blacklanternsecurity.com>
#
# Created:     06-07-2021
# Copyright:   (c) BLS OPS LLC. 2021
# Licence:     GPL
# -------------------------------------------------------------------------------
*/

using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enter_The_Matrix.FactorModels
{
    public class Techniques
    {
        public List<string> htmlContent { get; set; }
        private Dictionary<string, string> techniques { get; set; }
        public Dictionary<string, string> techniquesFull { get; set; }
        public Techniques(string selected)
        {
            #region techniquesFull
            techniquesFull = new Dictionary<string, string>();
            techniquesFull["T1595"] = "Active Scanning";
            techniquesFull["T1595/001"] = "Active Scanning: Scanning IP Blocks";
            techniquesFull["T1595/002"] = "Active Scanning: Vulnerability Scanning";
            techniquesFull["T1592"] = "Gather Victim Host Information";
            techniquesFull["T1592/001"] = "Gather Victim Host Information: Hardware";
            techniquesFull["T1592/002"] = "Gather Victim Host Information: Software";
            techniquesFull["T1592/003"] = "Gather Victim Host Information: Firmware";
            techniquesFull["T1592/004"] = "Gather Victim Host Information: Client Configurations";
            techniquesFull["T1589"] = "Gather Victim Identity Information";
            techniquesFull["T1589/001"] = "Gather Victim Identity Information: Credentials";
            techniquesFull["T1589/002"] = "Gather Victim Identity Information: Email Addresses";
            techniquesFull["T1589/003"] = "Gather Victim Identity Information: Employee Names";
            techniquesFull["T1590"] = "Gather Victim Network Information";
            techniquesFull["T1590/001"] = "Gather Victim Network Information: Domain Properties";
            techniquesFull["T1590/002"] = "Gather Victim Network Information: DNS";
            techniquesFull["T1590/003"] = "Gather Victim Network Information: Network Trust Dependencies";
            techniquesFull["T1590/004"] = "Gather Victim Network Information: Network Topology";
            techniquesFull["T1590/005"] = "Gather Victim Network Information: IP Addresses";
            techniquesFull["T1590/006"] = "Gather Victim Network Information: Network Security Appliances";
            techniquesFull["T1591"] = "Gather Victim Org Information";
            techniquesFull["T1591/001"] = "Gather Victim Org Information: Determine Physical Locations";
            techniquesFull["T1591/002"] = "Gather Victim Org Information: Business Relationships";
            techniquesFull["T1591/003"] = "Gather Victim Org Information: Identify Business Tempo";
            techniquesFull["T1591/004"] = "Gather Victim Org Information: Identify Roles";
            techniquesFull["T1598"] = "Phishing for Information";
            techniquesFull["T1598/001"] = "Phishing for Information: Spearphishing Service";
            techniquesFull["T1598/002"] = "Phishing for Information: Spearphishing Attachment";
            techniquesFull["T1598/003"] = "Phishing for Information: Spearphishing Link";
            techniquesFull["T1597"] = "Search Closed Sources";
            techniquesFull["T1597/001"] = "Search Closed Sources: Threat Intel Vendors";
            techniquesFull["T1597/002"] = "Search Closed Sources: Purchase Technical Data";
            techniquesFull["T1596"] = "Search Open Technical Databases";
            techniquesFull["T1596/001"] = "Search Open Technical Databases: DNS/Passive DNS";
            techniquesFull["T1596/002"] = "Search Open Technical Databases: WHOIS";
            techniquesFull["T1596/003"] = "Search Open Technical Databases: Digital Certificates";
            techniquesFull["T1596/004"] = "Search Open Technical Databases: CDNs";
            techniquesFull["T1596/005"] = "Search Open Technical Databases: Scan Databases";
            techniquesFull["T1593"] = "Search Open Websites/Domains";
            techniquesFull["T1593/001"] = "Search Open Websites/Domains: Social Media";
            techniquesFull["T1593/002"] = "Search Open Websites/Domains: Search Engines";
            techniquesFull["T1594"] = "Search Victim-Owned Websites";
            techniquesFull["T1583"] = "Acquire Infrastructure";
            techniquesFull["T1583/001"] = "Acquire Infrastructure: Domains";
            techniquesFull["T1583/002"] = "Acquire Infrastructure: DNS Server";
            techniquesFull["T1583/003"] = "Acquire Infrastructure: Virtual Private Server";
            techniquesFull["T1583/004"] = "Acquire Infrastructure: Server";
            techniquesFull["T1583/005"] = "Acquire Infrastructure: Botnet";
            techniquesFull["T1583/006"] = "Acquire Infrastructure: Web Services";
            techniquesFull["T1586"] = "Compromise Accounts";
            techniquesFull["T1586/001"] = "Compromise Accounts: Social Media Accounts";
            techniquesFull["T1586/002"] = "Compromise Accounts: Email Accounts";
            techniquesFull["T1584"] = "Compromise Infrastructure";
            techniquesFull["T1584/001"] = "Compromise Infrastructure: Domains";
            techniquesFull["T1584/002"] = "Compromise Infrastructure: DNS Server";
            techniquesFull["T1584/003"] = "Compromise Infrastructure: Virtual Private Server";
            techniquesFull["T1584/004"] = "Compromise Infrastructure: Server";
            techniquesFull["T1584/005"] = "Compromise Infrastructure: Botnet";
            techniquesFull["T1584/006"] = "Compromise Infrastructure: Web Services";
            techniquesFull["T1587"] = "Develop Capabilities";
            techniquesFull["T1587/001"] = "Develop Capabilities: Malware";
            techniquesFull["T1587/002"] = "Develop Capabilities: Code Signing Certificates";
            techniquesFull["T1587/003"] = "Develop Capabilities: Digital Certificates";
            techniquesFull["T1587/004"] = "Develop Capabilities: Exploits";
            techniquesFull["T1585"] = "Establish Accounts";
            techniquesFull["T1585/001"] = "Establish Accounts: Social Media Accounts";
            techniquesFull["T1585/002"] = "Establish Accounts: Email Accounts";
            techniquesFull["T1588"] = "Obtain Capabilities";
            techniquesFull["T1588/001"] = "Obtain Capabilities: Malware";
            techniquesFull["T1588/002"] = "Obtain Capabilities: Tool";
            techniquesFull["T1588/003"] = "Obtain Capabilities: Code Signing Certificates";
            techniquesFull["T1588/004"] = "Obtain Capabilities: Digital Certificates";
            techniquesFull["T1588/005"] = "Obtain Capabilities: Exploits";
            techniquesFull["T1588/006"] = "Obtain Capabilities: Vulnerabilities";
            techniquesFull["T1608"] = "Stage Capabilities";
            techniquesFull["T1608/001"] = "Stage Capabilities: Upload Malware";
            techniquesFull["T1608/002"] = "Stage Capabilities: Upload Tool";
            techniquesFull["T1608/003"] = "Stage Capabilities: Install Digital Certificate";
            techniquesFull["T1608/004"] = "Stage Capabilities: Drive-by Target";
            techniquesFull["T1608/005"] = "Stage Capabilities: Link Target";
            techniquesFull["T1189"] = "Drive-by Compromise";
            techniquesFull["T1190"] = "Exploit Public-Facing Application";
            techniquesFull["T1133"] = "External Remote Services";
            techniquesFull["T1200"] = "Hardware Additions";
            techniquesFull["T1566"] = "Phishing";
            techniquesFull["T1566/001"] = "Phishing: Spearphishing Attachment";
            techniquesFull["T1566/002"] = "Phishing: Spearphishing Link";
            techniquesFull["T1566/003"] = "Phishing: Spearphishing via Service";
            techniquesFull["T1091"] = "Replication Through Removable Media";
            techniquesFull["T1195"] = "Supply Chain Compromise";
            techniquesFull["T1195/001"] = "Supply Chain Compromise: Compromise Software Dependencies and Development Tools";
            techniquesFull["T1195/002"] = "Supply Chain Compromise: Compromise Software Supply Chain";
            techniquesFull["T1195/003"] = "Supply Chain Compromise: Compromise Hardware Supply Chain";
            techniquesFull["T1199"] = "Trusted Relationship";
            techniquesFull["T1078"] = "Valid Accounts";
            techniquesFull["T1078/001"] = "Valid Accounts: Default Accounts";
            techniquesFull["T1078/002"] = "Valid Accounts: Domain Accounts";
            techniquesFull["T1078/003"] = "Valid Accounts: Local Accounts";
            techniquesFull["T1078/004"] = "Valid Accounts: Cloud Accounts";
            techniquesFull["T1059"] = "Command and Scripting Interpreter";
            techniquesFull["T1059/001"] = "Command and Scripting Interpreter: PowerShell";
            techniquesFull["T1059/002"] = "Command and Scripting Interpreter: AppleScript";
            techniquesFull["T1059/003"] = "Command and Scripting Interpreter: Windows Command Shell";
            techniquesFull["T1059/004"] = "Command and Scripting Interpreter: Unix Shell";
            techniquesFull["T1059/005"] = "Command and Scripting Interpreter: Visual Basic";
            techniquesFull["T1059/006"] = "Command and Scripting Interpreter: Python";
            techniquesFull["T1059/007"] = "Command and Scripting Interpreter: JavaScript";
            techniquesFull["T1059/008"] = "Command and Scripting Interpreter: Network Device CLI";
            techniquesFull["T1609"] = "Container Administration Command";
            techniquesFull["T1610"] = "Deploy Container";
            techniquesFull["T1203"] = "Exploitation for Client Execution";
            techniquesFull["T1559"] = "Inter-Process Communication";
            techniquesFull["T1559/001"] = "Inter-Process Communication: Component Object Model";
            techniquesFull["T1559/002"] = "Inter-Process Communication: Dynamic Data Exchange";
            techniquesFull["T1106"] = "Native API";
            techniquesFull["T1053"] = "Scheduled Task/Job";
            techniquesFull["T1053/001"] = "Scheduled Task/Job: At (Linux)";
            techniquesFull["T1053/002"] = "Scheduled Task/Job: At (Windows)";
            techniquesFull["T1053/003"] = "Scheduled Task/Job: Cron";
            //techniquFulles["T1053/004"] = "Scheduled Task/Job: Launchd"; Deprecated v10
            techniquesFull["T1053/005"] = "Scheduled Task/Job: Scheduled Task";
            techniquesFull["T1053/006"] = "Scheduled Task/Job: Systemd Timers";
            techniquesFull["T1053/007"] = "Scheudled Task/Job: Container Orchestration Job";
            techniquesFull["T1129"] = "Shared Modules";
            techniquesFull["T1072"] = "Software Development Tools";
            techniquesFull["T1569"] = "System Services";
            techniquesFull["T1569/001"] = "System Services: Launchctl";
            techniquesFull["T1569/002"] = "System Services: Service Execution";
            techniquesFull["T1204"] = "User Execution";
            techniquesFull["T1204/001"] = "User Execution: Malicious Link";
            techniquesFull["T1204/002"] = "User Execution: Malicious File";
            techniquesFull["T1204/003"] = "User Execution: Malicious Image";
            techniquesFull["T1047"] = "Windows Management Instrumentation";
            techniquesFull["T1098"] = "Account Manipulation";
            techniquesFull["T1098/001"] = "Account Manipulation: Additional Cloud Credentials";
            techniquesFull["T1098/002"] = "Account Manipulation: Exchange Email Delegate Permissions";
            techniquesFull["T1098/003"] = "Account Manipulation: Add Office 365 Global Administrator Role";
            techniquesFull["T1098/004"] = "Account Manipulation: SSH Authorized Keys";
            techniquesFull["T1197"] = "BITS Jobs";
            techniquesFull["T1547"] = "Boot or Logon Autostart Execution";
            techniquesFull["T1547/001"] = "Boot or Logon Autostart Execution: Registry Run Keys / Startup Folder";
            techniquesFull["T1547/002"] = "Boot or Logon Autostart Execution: Authentication Package";
            techniquesFull["T1547/003"] = "Boot or Logon Autostart Execution: Time Providers";
            techniquesFull["T1547/004"] = "Boot or Logon Autostart Execution: Winlogon Helper DLL";
            techniquesFull["T1547/005"] = "Boot or Logon Autostart Execution: Security Support Provider";
            techniquesFull["T1547/006"] = "Boot or Logon Autostart Execution: Kernel Modules and Extensions";
            techniquesFull["T1547/007"] = "Boot or Logon Autostart Execution: Re-opened Applications";
            techniquesFull["T1547/008"] = "Boot or Logon Autostart Execution: LSASS Driver";
            techniquesFull["T1547/009"] = "Boot or Logon Autostart Execution: Shortcut Modification";
            techniquesFull["T1547/010"] = "Boot or Logon Autostart Execution: Port Monitors";
            techniquesFull["T1547/011"] = "Boot or Logon Autostart Execution: Plist Modification";
            techniquesFull["T1547/012"] = "Boot or Logon Autostart Execution: Print Processors";
            techniquesFull["T1547/013"] = "Boot or Logon Autostart Execution: XDG Autostart Entries";
            techniquesFull["T1547/014"] = "Boot or Logon Autostart Execution: Active Setup";
            techniquesFull["T1037"] = "Boot or Logon Initialization Scripts";
            techniquesFull["T1037/001"] = "Boot or Logon Initialization Scripts: Logon Script (Windows)";
            techniquesFull["T1037/002"] = "Boot or Logon Initialization Scripts: Logon Script (Linux)";
            techniquesFull["T1037/003"] = "Boot or Logon Initialization Scripts: Network Logon Script";
            techniquesFull["T1037/004"] = "Boot or Logon Initialization Scripts: RC Scripts";
            techniquesFull["T1037/005"] = "Boot or Logon Initialization Scripts: Startup Items";
            techniquesFull["T1176"] = "Browser Extensions";
            techniquesFull["T1554"] = "Compromise Client Software Binary";
            techniquesFull["T1136"] = "Create Account";
            techniquesFull["T1136/001"] = "Create Account: Local Account";
            techniquesFull["T1136/002"] = "Create Account: Domain Account";
            techniquesFull["T1136/003"] = "Create Account: Cloud Account";
            techniquesFull["T1543"] = "Create or Modify System Process";
            techniquesFull["T1543/001"] = "Create or Modify System Process: Launch Agent";
            techniquesFull["T1543/002"] = "Create or Modify System Process: Systemd Service";
            techniquesFull["T1543/003"] = "Create or Modify System Process: Windows Service";
            techniquesFull["T1543/004"] = "Create or Modify System Process: Launch Daemon";
            techniquesFull["T1546"] = "Event Triggered Execution";
            techniquesFull["T1546/001"] = "Event Triggered Execution: Change default file association";
            techniquesFull["T1546/002"] = "Event Triggered Execution: Screensaver";
            techniquesFull["T1546/003"] = "Event Triggered Execution: Windows Management Instrumentation Event Subscription";
            techniquesFull["T1546/004"] = "Event Triggered Execution: Unix Shell Configuration Modification";
            techniquesFull["T1546/005"] = "Event Triggered Execution: Trap";
            techniquesFull["T1546/006"] = "Event Triggered Execution: LC_LOAD_DYLIB Addition";
            techniquesFull["T1546/007"] = "Event Triggered Execution: Netsh helper DLL";
            techniquesFull["T1546/008"] = "Event Triggered Execution: Accessibility features";
            techniquesFull["T1546/009"] = "Event Triggered Execution: AppCert DLLs";
            techniquesFull["T1546/010"] = "Event Triggered Execution: AppInit DLLs";
            techniquesFull["T1546/011"] = "Event Triggered Execution: Application Shimming";
            techniquesFull["T1546/012"] = "Event Triggered Execution: Image File Execution Options Injection";
            techniquesFull["T1546/013"] = "Event Triggered Execution: PowerShell profile";
            techniquesFull["T1546/014"] = "Event Triggered Execution: Emond";
            techniquesFull["T1546/015"] = "Event Triggered Execution: Component Object Model Hijacking";
            techniquesFull["T1133"] = "External Remote Services";
            techniquesFull["T1574"] = "Hijack Execution Flow";
            techniquesFull["T1574/001"] = "Hijack Execution Flow: DLL Search Order Hijacking";
            techniquesFull["T1574/002"] = "Hijack Execution Flow: DLL Side-Loading";
            techniquesFull["T1574/004"] = "Hijack Execution Flow: Dylib Hijacking";
            techniquesFull["T1574/005"] = "Hijack Execution Flow: Executable Installer File Permissions Weakness";
            techniquesFull["T1574/006"] = "Hijack Execution Flow: Dynamic Linker Hijacking";
            techniquesFull["T1574/007"] = "Hijack Execution Flow: Path Interception by PATH Environment Variable";
            techniquesFull["T1574/008"] = "Hijack Execution Flow: Path Interception by Search Order Hijacking";
            techniquesFull["T1574/009"] = "Hijack Execution Flow: Path Interception by Unquoted Path";
            techniquesFull["T1574/010"] = "Hijack Execution Flow: Services File Permissions Weakness";
            techniquesFull["T1574/011"] = "Hijack Execution Flow: Services Registry Permissions Weakness";
            techniquesFull["T1574/012"] = "Hijack Execution Flow: COR_PROFILER";
            techniquesFull["T1525"] = "Implant Internal Image";
            techniquesFull["T1556"] = "Modify Authentication Process";
            techniquesFull["T1556/001"] = "Modify Authentication Process: Domain Controller Authentication";
            techniquesFull["T1556/002"] = "Modify Authentication Process: Password Filter DLL";
            techniquesFull["T1556/003"] = "Modify Authentication Process: Pluggable Authentication Modules";
            techniquesFull["T1556/004"] = "Modify Authentication Process: Network Device Authentication";
            techniquesFull["T1137"] = "Office Application Startup";
            techniquesFull["T1137/001"] = "Office Spplication Startup: Office Template Macros";
            techniquesFull["T1137/002"] = "Office Spplication Startup: Office Test";
            techniquesFull["T1137/003"] = "Office Spplication Startup: Outlook Forms";
            techniquesFull["T1137/004"] = "Office Spplication Startup: Outlook Home Page";
            techniquesFull["T1137/005"] = "Office Spplication Startup: Outlook Rules";
            techniquesFull["T1137/006"] = "Office Spplication Startup: Add-ins";
            techniquesFull["T1542"] = "Pre-OS boot";
            techniquesFull["T1542/001"] = "Pre-OS boot: System firmware";
            techniquesFull["T1542/002"] = "Pre-OS boot: Component firmware";
            techniquesFull["T1542/003"] = "Pre-OS boot: Bootkit";
            techniquesFull["T1542/004"] = "Pre-OS boot: ROMMONkit";
            techniquesFull["T1542/005"] = "Pre-OS boot: TFTP Boot";
            techniquesFull["T1053"] = "Scheduled Task/Job";
            techniquesFull["T1053/001"] = "Scheduled Task/Job: At (Linux)";
            techniquesFull["T1053/002"] = "Scheduled Task/Job: At (Windows)";
            techniquesFull["T1053/003"] = "Scheduled Task/Job: Cron";
            techniquesFull["T1053/005"] = "Scheduled Task/Job: Scheduled Task";
            techniquesFull["T1053/006"] = "Scheduled Task/Job: Systemd Timers";
            techniquesFull["T1053/007"] = "Scheduled Task/Job: Container Orchestration Job";
            techniquesFull["T1505"] = "Server Software Component";
            techniquesFull["T1505/001"] = "Server Software Component: SQL Stored Procedures";
            techniquesFull["T1505/002"] = "Server Software Component: Transport Agent";
            techniquesFull["T1505/003"] = "Server Software Component: Web Shell";
            techniquesFull["T1505/004"] = "Server Software Component: IIS Components";
            techniquesFull["T1205"] = "Traffic Signaling";
            techniquesFull["T1205/001"] = "Traffic Signaling: Port Knocking";
            techniquesFull["T1078"] = "Valid Accounts";
            techniquesFull["T1078/001"] = "Valid Accounts: Default Accounts";
            techniquesFull["T1078/002"] = "Valid Accounts: Domain Accounts";
            techniquesFull["T1078/003"] = "Valid Accounts: Local Accounts";
            techniquesFull["T1078/004"] = "Valid Accounts: Cloud Accounts";
            techniquesFull["T1548"] = "Abuse elevation control mechanism";
            techniquesFull["T1548/001"] = "Abuse elevation control mechanism: Setuid and Setgid";
            techniquesFull["T1548/002"] = "Abuse elevation control mechanism: Bypass user access control";
            techniquesFull["T1548/003"] = "Abuse elevation control mechanism: Sudo and sudo control";
            techniquesFull["T1548/004"] = "Abuse elevation control mechanism: Elevated execution with prompt";
            techniquesFull["T1134"] = "Access Token Manipulation";
            techniquesFull["T1134/001"] = "Access Token Manipulation: Token Impersonation/Theft";
            techniquesFull["T1134/002"] = "Access Token Manipulation: Create Process with Token";
            techniquesFull["T1134/003"] = "Access Token Manipulation: Make and Impersonate Token";
            techniquesFull["T1134/004"] = "Access Token Manipulation: Parent PID Spoofing";
            techniquesFull["T1134/005"] = "Access Token Manipulation: SID-History Injection";
            techniquesFull["T1547"] = "Boot or Logon Autostart Execution";
            techniquesFull["T1547/001"] = "Boot or Logon Autostart Execution: Registry Run Keys / Startup Folder";
            techniquesFull["T1547/002"] = "Boot or Logon Autostart Execution: Authenticaton Package";
            techniquesFull["T1547/003"] = "Boot or Logon Autostart Execution: Time Providers";
            techniquesFull["T1547/004"] = "Boot or Logon Autostart Execution: Winlogon Helper DLL";
            techniquesFull["T1547/005"] = "Boot or Logon Autostart Execution: Security Support Provider";
            techniquesFull["T1547/006"] = "Boot or Logon Autostart Execution: Kernel Modules and Extensions";
            techniquesFull["T1547/007"] = "Boot or Logon Autostart Execution: Re-opened Applications";
            techniquesFull["T1547/008"] = "Boot or Logon Autostart Execution: LSASS Driver";
            techniquesFull["T1547/009"] = "Boot or Logon Autostart Execution: Shortcut Modification";
            techniquesFull["T1547/010"] = "Boot or Logon Autostart Execution: Port Monitors";
            techniquesFull["T1547/011"] = "Boot or Logon Autostart Execution: Plist Modification";
            techniquesFull["T1547/012"] = "Boot or Logon Autostart Execution: Print Processors";
            techniquesFull["T1547/013"] = "Boot or Logon Autostart Execution: XDG Autostart Entries";
            techniquesFull["T1547/014"] = "Boot or Logon Autostart Execution: Active Setup";
            techniquesFull["T1037"] = "Boot or Logon Initialization Scripts";
            techniquesFull["T1037/001"] = "Boot or Logon Initialization Scripts: Logon Script (Windows)";
            techniquesFull["T1037/002"] = "Boot or Logon Initialization Scripts: Logon Script (Linux)";
            techniquesFull["T1037/003"] = "Boot or Logon Initialization Scripts: Network Logon Script";
            techniquesFull["T1037/004"] = "Boot or Logon Initialization Scripts: RC Scrits";
            techniquesFull["T1037/005"] = "Boot or Logon Initialization Scripts: Startup Items";
            techniquesFull["T1543"] = "Create or Modify System Process";
            techniquesFull["T1543/001"] = "Create or Modify System Process: Launch Agent";
            techniquesFull["T1543/002"] = "Create or Modify System Process: Systemd Service";
            techniquesFull["T1543/003"] = "Create or Modify System Process: Windows Service";
            techniquesFull["T1543/004"] = "Create or Modify System Process: Launch Daemon";
            techniquesFull["T1484"] = "Domain Policy Modification";
            techniquesFull["T1484/001"] = "Domain Policy Modification: Group Policy Modification";
            techniquesFull["T1484/002"] = "Domain Policy Modification: Domain Trust Modification";
            techniquesFull["T1611"] = "Escape to Host";
            techniquesFull["T1546"] = "Event Triggered Execution";
            techniquesFull["T1546/001"] = "Event Triggered Execution: Change default file association";
            techniquesFull["T1546/002"] = "Event Triggered Execution: Screensaver";
            techniquesFull["T1546/003"] = "Event Triggered Execution: Windows Management Instrumentation Event Subscription";
            techniquesFull["T1546/004"] = "Event Triggered Execution: Unix Shell Configuration Modification";
            techniquesFull["T1546/005"] = "Event Triggered Execution: Trap";
            techniquesFull["T1546/006"] = "Event Triggered Execution: LC_LOAD_DYLIB Addition";
            techniquesFull["T1546/007"] = "Event Triggered Execution: Netsh helper DLL";
            techniquesFull["T1546/008"] = "Event Triggered Execution: Accessibility features";
            techniquesFull["T1546/009"] = "Event Triggered Execution: AppCert DLLs";
            techniquesFull["T1546/010"] = "Event Triggered Execution: AppInit DLLs";
            techniquesFull["T1546/011"] = "Event Triggered Execution: Application Shimming";
            techniquesFull["T1546/012"] = "Event Triggered Execution: Image File Execution Options Injection";
            techniquesFull["T1546/013"] = "Event Triggered Execution: PowerShell profile";
            techniquesFull["T1546/014"] = "Event Triggered Execution: Emond";
            techniquesFull["T1546/015"] = "Event Triggered Execution: Component Object Model Hijacking";
            techniquesFull["T1068"] = "Exploitation for Privilege Escalation";
            techniquesFull["T1574"] = "Hijack Execution Flow";
            techniquesFull["T1574/001"] = "Hijack Execution Flow: DLL Search Order Hijacking";
            techniquesFull["T1574/002"] = "Hijack Execution Flow: DLL Side-Loading";
            techniquesFull["T1574/004"] = "Hijack Execution Flow: Dylib Hijacking";
            techniquesFull["T1574/005"] = "Hijack Execution Flow: Executable Installer File Permissions Weakness";
            techniquesFull["T1574/006"] = "Hijack Execution Flow: Dynamic Linker Hijacking";
            techniquesFull["T1574/007"] = "Hijack Execution Flow: Path Interception by PATH Environment Variable";
            techniquesFull["T1574/008"] = "Hijack Execution Flow: Path Interception by Search Order Hijacking";
            techniquesFull["T1574/009"] = "Hijack Execution Flow: Path Interception by Unquoted Path";
            techniquesFull["T1574/010"] = "Hijack Execution Flow: Services File Permissions Weakness";
            techniquesFull["T1574/011"] = "Hijack Execution Flow: Services Registry Permissions Weakness";
            techniquesFull["T1574/012"] = "Hijack Execution Flow: COR_PROFILER";
            techniquesFull["T1055"] = "Process Injection";
            techniquesFull["T1055/001"] = "Process Injection: Dynamic-link Library Injection";
            techniquesFull["T1055/002"] = "Process Injection: Portable Executable Injection";
            techniquesFull["T1055/003"] = "Process Injection: Thread Execution Hijacking";
            techniquesFull["T1055/004"] = "Process Injection: Asynchronous Procedure Call";
            techniquesFull["T1055/005"] = "Process Injection: Thread Local Storage";
            techniquesFull["T1055/008"] = "Process Injection: Ptrace System Calls";
            techniquesFull["T1055/009"] = "Process Injection: Proc Memory";
            techniquesFull["T1055/011"] = "Process Injection: Extra Window Memory Injection";
            techniquesFull["T1055/012"] = "Process Injection: Process Hollowing";
            techniquesFull["T1055/013"] = "Process Injection: Process Doppelganging";
            techniquesFull["T1055/014"] = "Process Injection: VDSO Hijacking";
            techniquesFull["T1053"] = "Scheduled Task/Job";
            techniquesFull["T1053/001"] = "Scheduled Task/Job: At (Linux)";
            techniquesFull["T1053/002"] = "Scheduled Task/Job: At (Windows)";
            techniquesFull["T1053/003"] = "Scheduled Task/Job: Cron";
            techniquesFull["T1053/005"] = "Scheduled Task/Job: Scheduled Task";
            techniquesFull["T1053/006"] = "Scheduled Task/Job: Systemd Timers";
            techniquesFull["T1053/007"] = "Scheduled Task/Job: Container Orchestration Job";
            techniquesFull["T1078"] = "Valid Accounts";
            techniquesFull["T1078/001"] = "Valid Accounts: Default Accounts";
            techniquesFull["T1078/002"] = "Valid Accounts: Domain Accounts";
            techniquesFull["T1078/003"] = "Valid Accounts: Local Accounts";
            techniquesFull["T1078/004"] = "Valid Accounts: Cloud Accounts";
            techniquesFull["T1548"] = "Abuse elevation control mechanism";
            techniquesFull["T1548/001"] = "Abuse elevation control mechanism: Setuid and setgid";
            techniquesFull["T1548/002"] = "Abuse elevation control mechanism: Bypass user access control";
            techniquesFull["T1548/003"] = "Abuse elevation control mechanism: Sudo and sudo caching";
            techniquesFull["T1548/004"] = "Abuse elevation control mechanism: Elevated execution with prompt";
            techniquesFull["T1134"] = "Access Token Manipulation";
            techniquesFull["T1134/001"] = "Access Token Manipulation: Token Impersonation/Theft";
            techniquesFull["T1134/002"] = "Access Token Manipulation: Create Process with Token";
            techniquesFull["T1134/003"] = "Access Token Manipulation: Make and Impersonate Token";
            techniquesFull["T1134/004"] = "Access Token Manipulation: Parent PID Spoofing";
            techniquesFull["T1134/005"] = "Access Token Manipulation: SID-History Injection";
            techniquesFull["T1197"] = "BITS Jobs";
            techniquesFull["T1610"] = "Deploy Container";
            techniquesFull["T1612"] = "Build Image on Host";
            techniquesFull["T1140"] = "Deobfuscate/decode files or information";
            techniquesFull["T1006"] = "Direct Volume Access";
            techniquesFull["T1480"] = "Execution Guardrails";
            techniquesFull["T1480/001"] = "Execution Guardrails: Environmental Keying";
            techniquesFull["T1211"] = "Exploitation for defense evasion";
            techniquesFull["T1222"] = "File and Directory Permissions Modificaton";
            techniquesFull["T1222/001"] = "File and Directory Permissions Modificaton: Windows File and Directory Permissions Modification";
            techniquesFull["T1222/002"] = "File and Directory Permissions Modificaton: Linux and Mac File and Directory Permissions Modification";
            techniquesFull["T1564"] = "Hide Artifacts";
            techniquesFull["T1564/001"] = "Hide Artifacts: Hidden Files and Directories";
            techniquesFull["T1564/002"] = "Hide Artifacts: Hidden Users";
            techniquesFull["T1564/003"] = "Hide Artifacts: Hidden Window";
            techniquesFull["T1564/004"] = "Hide Artifacts: NTFS File Attributes";
            techniquesFull["T1564/005"] = "Hide Artifacts: Hidden File System";
            techniquesFull["T1564/006"] = "Hide Artifacts: Run Virtual Instance";
            techniquesFull["T1564/007"] = "Hide Artifacts: VBA Stomping";
            techniquesFull["T1564/008"] = "Hide Artifacts: Email Hiding Rules";
            techniquesFull["T1564/009"] = "Hide Artifacts: Resource Forking";
            techniquesFull["T1574"] = "Hijack Execution Flow";
            techniquesFull["T1574/001"] = "Hijack Execution Flow: DLL Search Order Hijacking";
            techniquesFull["T1574/002"] = "Hijack Execution Flow: DLL Side-Loading";
            techniquesFull["T1574/004"] = "Hijack Execution Flow: Dylib Hijacking";
            techniquesFull["T1574/005"] = "Hijack Execution Flow: Executable Intaller File Permissions Weakness";
            techniquesFull["T1574/006"] = "Hijack Execution Flow: Dynamic Linker Hijacking";
            techniquesFull["T1574/007"] = "Hijack Execution Flow: Path Interception by PATH Environment Variable";
            techniquesFull["T1574/008"] = "Hijack Execution Flow: Path Interception by Search Order Hijacking";
            techniquesFull["T1574/009"] = "Hijack Execution Flow: Path Interception by Unquoted Path";
            techniquesFull["T1574/010"] = "Hijack Execution Flow: Services File Permissions Weakness";
            techniquesFull["T1574/011"] = "Hijack Execution Flow: Services Registry Permissions Weakness";
            techniquesFull["T1574/012"] = "Hijack Execution Flow: COR_PROFILER";
            techniquesFull["T1562"] = "Impair Defenses";
            techniquesFull["T1562/001"] = "Impair Defenses: Disable or Modify Tools";
            techniquesFull["T1562/002"] = "Impair Defenses: Disable Windows Event Logging";
            techniquesFull["T1562/003"] = "Impair Defenses: Impair Command History Logging";
            techniquesFull["T1562/004"] = "Impair Defenses: Disable or Modify System Firewall";
            techniquesFull["T1562/006"] = "Impair Defenses: Indicator Blocking";
            techniquesFull["T1562/007"] = "Impair Defenses: Disable or Modify Cloud Firewall";
            techniquesFull["T1562/008"] = "Impair Defenses: Disable Cloud Logs";
            techniquesFull["T1562/009"] = "Impair Defenses: Safe Mode Boot";
            techniquesFull["T1562/010"] = "Impair Defenses: Downgrade Attack";
            techniquesFull["T1070"] = "Indicator Removal on Host";
            techniquesFull["T1070/001"] = "Indicator Removal on Host: Clear Windows Event Logs";
            techniquesFull["T1070/002"] = "Indicator Removal on Host: Clear Linux or Mac System Logs";
            techniquesFull["T1070/003"] = "Indicator Removal on Host: Clear Command History";
            techniquesFull["T1070/004"] = "Indicator Removal on Host: File Deletion";
            techniquesFull["T1070/005"] = "Indicator Removal on Host: Network Share Connection Removal";
            techniquesFull["T1070/006"] = "Indicator Removal on Host: Timestomp";
            techniquesFull["T1202"] = "Indirect Command Execution";
            techniquesFull["T1036"] = "Masquerading";
            techniquesFull["T1036/001"] = "Masquerading: Invalid Code Signature";
            techniquesFull["T1036/002"] = "Masquerading: Right-to-Left Override";
            techniquesFull["T1036/003"] = "Masquerading: Rename System Utilities";
            techniquesFull["T1036/004"] = "Masquerading: Masquerade Task or Service";
            techniquesFull["T1036/005"] = "Masquerading: Match Legitimate Name or Location";
            techniquesFull["T1036/006"] = "Masquerading: Space after Filename";
            techniquesFull["T1036/007"] = "Masquerading: Double File Extension";
            techniquesFull["T1556"] = "Modify Authentication Process";
            techniquesFull["T1556/001"] = "Modify Authentication Process: Domain Controller Authentication";
            techniquesFull["T1556/002"] = "Modify Authentication Process: Password Filter DLL";
            techniquesFull["T1556/003"] = "Modify Authentication Process: Pluggable Authentication Modules";
            techniquesFull["T1556/004"] = "Modify Authentication Process: Network Device Authentication";
            techniquesFull["T1578"] = "Modify Cloud Compute Infrastructure";
            techniquesFull["T1578/001"] = "Modify Cloud Compute Infrastructure: Create Snapshot";
            techniquesFull["T1578/002"] = "Modify Cloud Compute Infrastructure: Create Cloud Instance";
            techniquesFull["T1578/003"] = "Modify Cloud Compute Infrastructure: Delete Cloud Instance";
            techniquesFull["T1578/004"] = "Modify Cloud Compute Infrastructure: Revert Cloud Instance";
            techniquesFull["T1112"] = "Modify Registry";
            techniquesFull["T1601"] = "Modify System Image";
            techniquesFull["T1601/001"] = "Modify System Image: Patch System Image";
            techniquesFull["T1601/002"] = "Modify System Image: Downgrade System Image";
            techniquesFull["T1599"] = "Network Boundary Bridging";
            techniquesFull["T1599/001"] = "Network Boundary Bridging: Network Address Translation Traversal";
            techniquesFull["T1027"] = "Obfuscated Files or Information";
            techniquesFull["T1027/001"] = "Obfuscated Files or Information: Binary Padding";
            techniquesFull["T1027/002"] = "Obfuscated Files or Information: Software Packing";
            techniquesFull["T1027/003"] = "Obfuscated Files or Information: Steganography";
            techniquesFull["T1027/004"] = "Obfuscated Files or Information: Compile after Delivery";
            techniquesFull["T1027/005"] = "Obfuscated Files or Information: Indicator Removal from Tools";
            techniquesFull["T1027/006"] = "Obfuscated Files or Information: HTML Smuggling";
            techniquesFull["T1542"] = "Pre-OS boot";
            techniquesFull["T1542/001"] = "Pre-OS boot: System Firmware";
            techniquesFull["T1542/002"] = "Pre-OS boot: Component Firmware";
            techniquesFull["T1542/003"] = "Pre-OS boot: Bootkit";
            techniquesFull["T1542/004"] = "Pre-OS boot: ROMMONkit";
            techniquesFull["T1542/005"] = "Pre-OS boot: TFTP Boot";
            techniquesFull["T1055"] = "Process Injection";
            techniquesFull["T1055/001"] = "Process Injection: Dynamic-link Library Injection";
            techniquesFull["T1055/002"] = "Process Injection: Portable Executable Injection";
            techniquesFull["T1055/003"] = "Process Injection: Thread Execution Hijacking";
            techniquesFull["T1055/004"] = "Process Injection: Asynchronous Procedure Call";
            techniquesFull["T1055/005"] = "Process Injection: Thread Local Storage";
            techniquesFull["T1055/008"] = "Process Injection: Ptrace System Calls";
            techniquesFull["T1055/009"] = "Process Injection: Proc Memory";
            techniquesFull["T1055/011"] = "Process Injection: Extra Window Memory Injection";
            techniquesFull["T1055/012"] = "Process Injection: Process Hollowing";
            techniquesFull["T1055/013"] = "Process Injection: Process Doppelganging";
            techniquesFull["T1055/014"] = "Process Injection: VDSO Hijacking";
            techniquesFull["T1207"] = "Rogue Domain Controller";
            techniquesFull["T1014"] = "Rootkit";
            techniquesFull["T1218"] = "Signed Binary Proxy Execution";
            techniquesFull["T1218/001"] = "Signed Binary Proxy Execution: Compiled HTML File";
            techniquesFull["T1218/002"] = "Signed Binary Proxy Execution: Control Panel";
            techniquesFull["T1218/003"] = "Signed Binary Proxy Execution: CMSTP";
            techniquesFull["T1218/004"] = "Signed Binary Proxy Execution: InstallUtil";
            techniquesFull["T1218/005"] = "Signed Binary Proxy Execution: Mshta";
            techniquesFull["T1218/007"] = "Signed Binary Proxy Execution: Msiexec";
            techniquesFull["T1218/008"] = "Signed Binary Proxy Execution: Odbcconf";
            techniquesFull["T1218/009"] = "Signed Binary Proxy Execution: Regsvcs/Regasm";
            techniquesFull["T1218/010"] = "Signed Binary Proxy Execution: Regsvr32";
            techniquesFull["T1218/011"] = "Signed Binary Proxy Execution: Rundll32";
            techniquesFull["T1218/012"] = "Signed Binary Proxy Execution: Verclsid";
            techniquesFull["T1218/013"] = "Signed Binary Proxy Execution: Mavinject";
            techniquesFull["T1218/014"] = "Signed Binary Proxy Execution: MMC";
            techniquesFull["T1216"] = "Signed Script Proxy Execution";
            techniquesFull["T1216/001"] = "Signed Script Proxy Execution: PubPrn";
            techniquesFull["T1553"] = "Subvert Trust Controls";
            techniquesFull["T1553/001"] = "Subvert Trust Controls: Gatekeeper Bypass";
            techniquesFull["T1553/002"] = "Subvert Trust Controls: Code Signing";
            techniquesFull["T1553/003"] = "Subvert Trust Controls: SIP and Trust Provider Hijacking";
            techniquesFull["T1553/004"] = "Subvert Trust Controls: Install Root Certificate";
            techniquesFull["T1553/005"] = "Subvert Trust Controls: Mark-of-the-Web Bypass";
            techniquesFull["T1553/006"] = "Subvert Trust Controls: Code Signing Policy Modificaiton";
            techniquesFull["T1221"] = "Template Injection";
            techniquesFull["T1205"] = "Traffic Signaling";
            techniquesFull["T1205/001"] = "Traffic Signaling: Port Knocking";
            techniquesFull["T1127"] = "Trusted Developer Utilities Proxy Execution";
            techniquesFull["T1127/001"] = "Trusted Developer Utilities Proxy Execution: MSBuild";
            techniquesFull["T1535"] = "Unused/Unsupported Cloud Regions";
            techniquesFull["T1550"] = "Use Alternative Authentication Material";
            techniquesFull["T1550/001"] = "Use Alternative Authentication Material: Application Access Token";
            techniquesFull["T1550/002"] = "Use Alternative Authentication Material: Pass the Hash";
            techniquesFull["T1550/003"] = "Use Alternative Authentication Material: Pass the Ticket";
            techniquesFull["T1550/004"] = "Use Alternative Authentication Material: Web Session Cookie";
            techniquesFull["T1078"] = "Valid Accounts";
            techniquesFull["T1078/001"] = "Valid Accounts: Default Accounts";
            techniquesFull["T1078/002"] = "Valid Accounts: Domain Accounts";
            techniquesFull["T1078/003"] = "Valid Accounts: Local Accounts";
            techniquesFull["T1078/004"] = "Valid Accounts: Cloud Accounts";
            techniquesFull["T1497"] = "Virtualization/Sandbox Evasion";
            techniquesFull["T1497/001"] = "Virtualization/Sandbox Evasion: System Checks";
            techniquesFull["T1497/002"] = "Virtualization/Sandbox Evasion: User Activity Based Checks";
            techniquesFull["T1497/003"] = "Virtualization/Sandbox Evasion: Time Based Evasion";
            techniquesFull["T1600"] = "Weaken Encryption";
            techniquesFull["T1600/001"] = "Weaken Encryption: Reduce Key Space";
            techniquesFull["T1600/002"] = "Weaken Encryption: Disable Crypto Hardware";
            techniquesFull["T1220"] = "XSL Script Processing";
            techniquesFull["T1484"] = "Domain Policy Modification";
            techniquesFull["T1484/001"] = "Domain Policy Modification: Group Policy Modification";
            techniquesFull["T1484/002"] = "Domain Policy Modification: Domain Trust Modification";
            techniquesFull["T1110"] = "Brute Force";
            techniquesFull["T1110/001"] = "Brute Force: Password Guessing";
            techniquesFull["T1110/002"] = "Brute Force: Password Cracking";
            techniquesFull["T1110/003"] = "Brute Force: Password Spraying";
            techniquesFull["T1110/004"] = "Brute Force: Credential Stuffing";
            techniquesFull["T1555"] = "Credentials from Password Stores";
            techniquesFull["T1555/001"] = "Credentials from Password Stores: Keychain";
            techniquesFull["T1555/002"] = "Credentials from Password Stores: Securityd Memory";
            techniquesFull["T1555/003"] = "Credentials from Password Stores: Credentials from Web Browsers";
            techniquesFull["T1555/004"] = "Credentials from Password Stores: Windows Credential Manager";
            techniquesFull["T1555/005"] = "Credentials from Password Stores: Password Managers";
            techniquesFull["T1212"] = "Exploitation for credential access";
            techniquesFull["T1187"] = "Forced authentication";
            techniquesFull["T1056"] = "Input Capture";
            techniquesFull["T1056/001"] = "Input Capture: Keylogging";
            techniquesFull["T1056/002"] = "Input Capture: GUI Input Capture";
            techniquesFull["T1056/003"] = "Input Capture: Web Portal Capture";
            techniquesFull["T1056/004"] = "Input Capture: Credential API Hooking";
            techniquesFull["T1557"] = "Adversary-in-the-Middle";
            techniquesFull["T1557/001"] = "Adversary-in-the-Middle: LLMNR/NBT-NS poisoning and SMB relay";
            techniquesFull["T1557/002"] = "Adversary-in-the-Middle: ARP Cache Poisoning";
            techniquesFull["T1556"] = "Modify Authentication Process";
            techniquesFull["T1556/001"] = "Modify Authentication Process: Domain Controller Authentication";
            techniquesFull["T1556/002"] = "Modify Authentication Process: Password Filter DLL";
            techniquesFull["T1556/003"] = "Modify Authentication Process: Pluggable Authentication Modules";
            techniquesFull["T1556/004"] = "Modify Authentication Process: Network Device Authentication";
            techniquesFull["T1040"] = "Network Sniffing";
            techniquesFull["T1003"] = "OS Credential Dumping";
            techniquesFull["T1003/001"] = "OS Credential Dumping: LSASS Memory";
            techniquesFull["T1003/002"] = "OS Credential Dumping: Security Account Manager";
            techniquesFull["T1003/003"] = "OS Credential Dumping: NTDS";
            techniquesFull["T1003/004"] = "OS Credential Dumping: LSA Secrets";
            techniquesFull["T1003/005"] = "OS Credential Dumping: Cached Domain Credentials";
            techniquesFull["T1003/006"] = "OS Credential Dumping: DCSync";
            techniquesFull["T1003/007"] = "OS Credential Dumping: Proc Filesystem";
            techniquesFull["T1003/008"] = "OS Credential Dumping: /etc/passwd and /etc/shadow";
            techniquesFull["T1528"] = "Steal Application Access Token";
            techniquesFull["T1558"] = "Steal or Forge Kerberos Tickets";
            techniquesFull["T1558/001"] = "Steal or Forge Kerberos Tickets: Golden Ticket";
            techniquesFull["T1558/002"] = "Steal or Forge Kerberos Tickets: Silver Ticket";
            techniquesFull["T1558/003"] = "Steal or Forge Kerberos Tickets: Kerberoasting";
            techniquesFull["T1558/004"] = "Steal or Forge Kerberos Tickets: AS-REP Roasting";
            techniquesFull["T1539"] = "Steal Web Session Cookie";
            techniquesFull["T1111"] = "Two-Factor Authentication Interception";
            techniquesFull["T1552"] = "Unsecured Credentials";
            techniquesFull["T1552/001"] = "Unsecured Credentials: Credentials in Files";
            techniquesFull["T1552/002"] = "Unsecured Credentials: Credentials in Registry";
            techniquesFull["T1552/003"] = "Unsecured Credentials: Bash History";
            techniquesFull["T1552/004"] = "Unsecured Credentials: Private Keys";
            techniquesFull["T1552/005"] = "Unsecured Credentials: Cloud Instance Metadata API";
            techniquesFull["T1552/006"] = "Unsecured Credentials: Group Policy Preferences";
            techniquesFull["T1552/007"] = "Unsecured Credentials: Container API";
            techniquesFull["T1606"] = "Forge Web Credentials";
            techniquesFull["T1606/001"] = "Forge Web Credentials: Web Cookies";
            techniquesFull["T1606/002"] = "Forge Web Credentials: SAML Tokens";
            techniquesFull["T1087"] = "Account Discovery";
            techniquesFull["T1087/001"] = "Account Discovery: Local Account";
            techniquesFull["T1087/002"] = "Account Discovery: Domain Account";
            techniquesFull["T1087/003"] = "Account Discovery: Email Account";
            techniquesFull["T1087/004"] = "Account Discovery: Cloud Account";
            techniquesFull["T1010"] = "Application Window Discovery";
            techniquesFull["T1217"] = "Browser Bookmark Discovery";
            techniquesFull["T1580"] = "Cloud Infrastructure Discovery";
            techniquesFull["T1538"] = "Cloud Service Dashboard";
            techniquesFull["T1526"] = "Cloud Service Discovery";
            techniquesFull["T1613"] = "Container and Resource Discovery";
            techniquesFull["T1482"] = "Domain Trust Discovery";
            techniquesFull["T1083"] = "File and Directory Discovery";
            techniquesFull["T1046"] = "Network Service Scanning";
            techniquesFull["T1135"] = "Network Share Discovery";
            techniquesFull["T1040"] = "Network Sniffing";
            techniquesFull["T1201"] = "Password Policy Discovery";
            techniquesFull["T1120"] = "Peripheral Device Discovery";
            techniquesFull["T1069"] = "Permission Groups Discovery";
            techniquesFull["T1069/001"] = "Permission Groups Discovery: Local Groups";
            techniquesFull["T1069/002"] = "Permission Groups Discovery: Domain Groups";
            techniquesFull["T1069/003"] = "Permission Groups Discovery: Cloud Groups";
            techniquesFull["T1057"] = "Process Discovery";
            techniquesFull["T1012"] = "Query Registry";
            techniquesFull["T1018"] = "Remote System Discovery";
            techniquesFull["T1518"] = "Software Discovery";
            techniquesFull["T1518/001"] = "Software Discovery: Security Software Discovery";
            techniquesFull["T1082"] = "System Information Discovery";
            techniquesFull["T1614"] = "System Location Discovery";
            techniquesFull["T1614/001"] = "System Location Discovery: System Language Discovery";
            techniquesFull["T1016"] = "System Network Configuration Discovery";
            techniquesFull["T1016/001"] = "System Network Configuration Discovery: Internet Connection Discovery";
            techniquesFull["T1049"] = "System Network Connections Discovery";
            techniquesFull["T1033"] = "System Owner/User Discovery";
            techniquesFull["T1007"] = "System Service Discovery";
            techniquesFull["T1124"] = "System Time Discovery";
            techniquesFull["T1497"] = "Virtualization/Sandbox Evasion";
            techniquesFull["T1497/001"] = "Virtualization/Sandbox Evasion: System Checks";
            techniquesFull["T1497/002"] = "Virtualization/Sandbox Evasion: User Activity Based Checks";
            techniquesFull["T1497/003"] = "Virtualization/Sandbox Evasion: Time Based Evasion";
            techniquesFull["T1210"] = "Exploitation of Remote Services";
            techniquesFull["T1534"] = "Internal Spearphishing";
            techniquesFull["T1570"] = "Lateral Tool Transfer";
            techniquesFull["T1563"] = "Remote Service Session Hijacking";
            techniquesFull["T1563/001"] = "Remote Service Session Hijacking: SSH Hijacking";
            techniquesFull["T1563/002"] = "Remote Service Session Hijacking: RDP Hijacking";
            techniquesFull["T1021"] = "Remote Services";
            techniquesFull["T1021/001"] = "Remote Services: Remote Desktop Protocol";
            techniquesFull["T1021/002"] = "Remote Services: SMB/Windows Admin Shares";
            techniquesFull["T1021/003"] = "Remote Services: Distributed Component Object Model";
            techniquesFull["T1021/004"] = "Remote Services: SSH";
            techniquesFull["T1021/005"] = "Remote Services: VNC";
            techniquesFull["T1021/006"] = "Remote Services: Windows Remote Management";
            techniquesFull["T1091"] = "Replication Through Removable Media";
            techniquesFull["T1072"] = "Software Deployment Tools";
            techniquesFull["T1080"] = "Taint Shared Content";
            techniquesFull["T1550"] = "Use Alternate Authentication Material";
            techniquesFull["T1550/001"] = "Use Alternate Authentication Material: Application Access Token";
            techniquesFull["T1550/002"] = "Use Alternate Authentication Material: Pass the Hash";
            techniquesFull["T1550/003"] = "Use Alternate Authentication Material: Pass the Ticket";
            techniquesFull["T1550/004"] = "Use Alternate Authentication Material: Web Session Cookie";
            techniquesFull["T1560"] = "Archive Collected Data";
            techniquesFull["T1560/001"] = "Archive Collected Data: Archive via Utility";
            techniquesFull["T1560/002"] = "Archive Collected Data: Archive via Library";
            techniquesFull["T1560/003"] = "Archive Collected Data: Archive via Custom Method";
            techniquesFull["T1123"] = "Audio Capture";
            techniquesFull["T1119"] = "Automated Collection";
            techniquesFull["T1115"] = "Clipboard Data";
            techniquesFull["T1530"] = "Data from Cloud Storage Object";
            techniquesFull["T1602"] = "Data from Configuration Repository";
            techniquesFull["T1602/001"] = "Data from Configuration Repository: SNMP (MIB Dump)";
            techniquesFull["T1602/002"] = "Data from Configuration Repository: Network Device Configuration Dump";
            techniquesFull["T1213"] = "Data from Information Repositories";
            techniquesFull["T1213/001"] = "Data from Information Repositories: Confluence";
            techniquesFull["T1213/002"] = "Data from Information Repositories: Sharepoint";
            techniquesFull["T1213/003"] = "Data from Information Repositories: Code Repositories";
            techniquesFull["T1005"] = "Data from Local System";
            techniquesFull["T1039"] = "Data from Network Shared Drive";
            techniquesFull["T1025"] = "Data from Removable Media";
            techniquesFull["T1074"] = "Data Staged";
            techniquesFull["T1074/001"] = "Data Staged: Local Data Staging";
            techniquesFull["T1074/002"] = "Data Staged: Remote Data Staging";
            techniquesFull["T1114"] = "Email Collection";
            techniquesFull["T1114/001"] = "Email Collection: Local Email Collection";
            techniquesFull["T1114/002"] = "Email Collection: Remote Email Collection";
            techniquesFull["T1114/003"] = "Email Collection: Email Forwarding Rule";
            techniquesFull["T1056"] = "Input Capture";
            techniquesFull["T1056/001"] = "Input Capture: Keylogging";
            techniquesFull["T1056/002"] = "Input Capture: GUI input capture";
            techniquesFull["T1056/003"] = "Input Capture: Web portal capture";
            techniquesFull["T1056/004"] = "Input Capture: Credential API hooking";
            techniquesFull["T1185"] = "Browser Session Hijacking";
            techniquesFull["T1557"] = "Adversary-in-the-Middle";
            techniquesFull["T1557/001"] = "Adversary-in-the-Middle: LLMNR/NBT-NS Poisoning and SMB Relay";
            techniquesFull["T1557/002"] = "Adversary-in-the-Middle: ARP Cache Poisoning";
            techniquesFull["T1113"] = "Screen Capture";
            techniquesFull["T1125"] = "Video Capture";
            techniquesFull["T1071"] = "Application layer protocol";
            techniquesFull["T1071/001"] = "Application layer protocol: Web protocols";
            techniquesFull["T1071/002"] = "Application layer protocol: File transfer protocols";
            techniquesFull["T1071/003"] = "Application layer protocol: Mail protocols";
            techniquesFull["T1071/004"] = "Application layer protocol: DNS";
            techniquesFull["T1092"] = "Communication through removable media";
            techniquesFull["T1132"] = "Data encoding";
            techniquesFull["T1132/001"] = "Data encoding: Standard encoding";
            techniquesFull["T1132/002"] = "Data encoding: Non-standard encoding";
            techniquesFull["T1001"] = "Data obfuscation";
            techniquesFull["T1001/001"] = "Data obfuscation: Junk data";
            techniquesFull["T1001/002"] = "Data obfuscation: Steganography";
            techniquesFull["T1001/003"] = "Data obfuscation: Protocol impersonation";
            techniquesFull["T1568"] = "Dynamic resolution";
            techniquesFull["T1568/001"] = "Dynamic resolution: Fast Flux DNS";
            techniquesFull["T1568/002"] = "Dynamic resolution: Domain Generation Algorithms";
            techniquesFull["T1568/003"] = "Dynamic resolution: DNS Calculation";
            techniquesFull["T1573"] = "Encrypted Channel";
            techniquesFull["T1573/001"] = "Encrypted Channel: Symmetric Cryptography";
            techniquesFull["T1573/002"] = "Encrypted Channel: Asymmetric cryptography";
            techniquesFull["T1008"] = "Fallback channels";
            techniquesFull["T1105"] = "Ingress tool transfer";
            techniquesFull["T1104"] = "Multi-stage channels";
            techniquesFull["T1095"] = "Non-application layer protocol";
            techniquesFull["T1571"] = "Non-standard port";
            techniquesFull["T1572"] = "Protocol tunneling";
            techniquesFull["T1090"] = "Proxy";
            techniquesFull["T1090/001"] = "Proxy: Internal proxy";
            techniquesFull["T1090/002"] = "Proxy: External proxy";
            techniquesFull["T1090/003"] = "Proxy: Multi-hop proxy";
            techniquesFull["T1090/004"] = "Proxy: Domain fronting";
            techniquesFull["T1219"] = "Remote Access Software";
            techniquesFull["T1205"] = "Traffic Signaling";
            techniquesFull["T1205/001"] = "Traffic Signaling: Port Knocking";
            techniquesFull["T1102"] = "Web service";
            techniquesFull["T1102/001"] = "Web service: Dead drop resolver";
            techniquesFull["T1102/002"] = "Web service: Bidirectional communication";
            techniquesFull["T1102/003"] = "Web service: One-way communication";
            techniquesFull["T1020"] = "Automated Exfiltration";
            techniquesFull["T1020/001"] = "Automated Exfiltration: Traffic Duplication";
            techniquesFull["T1030"] = "Data trasnfer size limits";
            techniquesFull["T1048"] = "Exfiltration Over Alternative Protocol";
            techniquesFull["T1048/001"] = "Exfiltration Over Alternative Protocol: Exfiltration Over Symmetric Encrypted Non-C2 Protocol";
            techniquesFull["T1048/002"] = "Exfiltration Over Alternative Protocol: Exfiltration Over Asymmetric Encrypted Non-C2 Protocol";
            techniquesFull["T1048/003"] = "Exfiltration Over Alternative Protocol: Exfiltration Over Unencrypted/Obfuscated Non-C2 Protocol";
            techniquesFull["T1041"] = "Exfiltration Over C2 Channel";
            techniquesFull["T1011"] = "Exfiltration over other network medium";
            techniquesFull["T1011/001"] = "Exfiltration over other network medium: Exfiltration over bluetooth";
            techniquesFull["T1052"] = "Exfiltration Over Physical Medium";
            techniquesFull["T1052/001"] = "Exfiltration Over Physical Medium: Exfiltration Over USB";
            techniquesFull["T1567"] = "Exfiltration Over Web Service";
            techniquesFull["T1567/001"] = "Exfiltration Over Web Service: Exfiltration to Code Repository";
            techniquesFull["T1567/002"] = "Exfiltration Over Web Service: Exfiltration to Cloud Storage";
            techniquesFull["T1029"] = "Scheduled transfer";
            techniquesFull["T1537"] = "Transfer Data to Cloud Account";
            techniquesFull["T1531"] = "Account Access Removal";
            techniquesFull["T1485"] = "Data Destruction";
            techniquesFull["T1486"] = "Data Encrypted for Impact";
            techniquesFull["T1565"] = "Data Manipulation";
            techniquesFull["T1565/001"] = "Data Manipulation: Stored Data Manipulation";
            techniquesFull["T1565/002"] = "Data Manipulation: Transmitted Data Manipulation";
            techniquesFull["T1565/003"] = "Data Manipulation: Runtime Data Manipulation";
            techniquesFull["T1491"] = "Defacement";
            techniquesFull["T1491/001"] = "Defacement: Internal Defacement";
            techniquesFull["T1491/002"] = "Defacement: External Defacement";
            techniquesFull["T1561"] = "Disk Wipe";
            techniquesFull["T1561/001"] = "Disk Wipe: Disk Content Wipe";
            techniquesFull["T1561/002"] = "Disk Wipe: Disk Structure Wipe";
            techniquesFull["T1499"] = "Endpoint Denial of Service";
            techniquesFull["T1499/001"] = "Endpoint Denial of Service: OS Exhaustion Flood";
            techniquesFull["T1499/002"] = "Endpoint Denial of Service: Service Exhaustion Flood";
            techniquesFull["T1499/003"] = "Endpoint Denial of Service: Application Exhaustion Flood";
            techniquesFull["T1499/004"] = "Endpoint Denial of Service: Application or System Exploitation";
            techniquesFull["T1495"] = "Firmware Corruption";
            techniquesFull["T1490"] = "Inhibit System Recovery";
            techniquesFull["T1498"] = "Network Denial of Service";
            techniquesFull["T1498/001"] = "Network Denial of Service: Direct Network Flood";
            techniquesFull["T1498/002"] = "Network Denial of Service: Reflection Amplification";
            techniquesFull["T1496"] = "Resource Hijacking";
            techniquesFull["T1489"] = "Service Stop";
            techniquesFull["T1529"] = "System Shutdown/Reboot";
            techniquesFull["T1475"] = "Deliver malicious app via authorized app store";
            techniquesFull["T1476"] = "Deliver malicious app via other means";
            techniquesFull["T1456"] = "Drive-by compromise";
            techniquesFull["T1458"] = "Exploit via charging station or PC";
            techniquesFull["T1477"] = "Exploit via radio interfaces";
            techniquesFull["T1478"] = "Install insecure of malicious configuration";
            techniquesFull["T1461"] = "Lockscreen bypass";
            techniquesFull["T1444"] = "Masquerade as legitimate application";
            techniquesFull["T1474"] = "Supply chain compromise";
            techniquesFull["T1402"] = "Broadcast receivers";
            techniquesFull["T1575"] = "Native code";
            techniquesFull["T1605"] = "Command-Line Interface";
            techniquesFull["T1603"] = "Scheduled Task/Job";
            techniquesFull["T1401"] = "Abuse device administrator access to prevent removal";
            techniquesFull["T1402"] = "Broadcast receivers";
            techniquesFull["T1540"] = "Code injection";
            techniquesFull["T1577"] = "Compromise application executable";
            techniquesFull["T1541"] = "Foreground persistence";
            techniquesFull["T1403"] = "Modify cached executable code";
            techniquesFull["T1398"] = "Modify OS kernal or boot partition";
            techniquesFull["T1400"] = "Modify system partition";
            techniquesFull["T1399"] = "Modify trusted execution environment";
            techniquesFull["T1603"] = "Scheduled Task/Job";
            techniquesFull["T1540"] = "Code injection";
            techniquesFull["T1401"] = "Device Administrator Permissions";
            techniquesFull["T1404"] = "Exploit OS vulnerability";
            techniquesFull["T1405"] = "Exploit TEE vulnerability";
            techniquesFull["T1418"] = "Application discovery";
            techniquesFull["T1540"] = "Code injection";
            techniquesFull["T1447"] = "Delete Device Data";
            techniquesFull["T1446"] = "Device lockout";
            techniquesFull["T1408"] = "Disguise root/jailbreak indicators";
            techniquesFull["T1407"] = "Download new code at runtime";
            techniquesFull["T1523"] = "Evade analysis environment";
            techniquesFull["T1581"] = "Geofencing";
            techniquesFull["T1516"] = "Input injection";
            techniquesFull["T1478"] = "Install insecure or malicious configuration";
            techniquesFull["T1444"] = "Masquerade as legitimate application";
            techniquesFull["T1398"] = "Modify OS kernel or boot partition";
            techniquesFull["T1400"] = "Modify system partition";
            techniquesFull["T1399"] = "Modify trusted execution environment";
            techniquesFull["T1575"] = "Native code";
            techniquesFull["T1406"] = "Obfuscated files or information";
            techniquesFull["T1508"] = "Suppress application icon";
            techniquesFull["T1576"] = "Uninstall malicious application";
            techniquesFull["T1604"] = "Proxy Through Victim";
            techniquesFull["T1517"] = "Access notifications";
            techniquesFull["T1413"] = "Access sensitive data in device logs";
            techniquesFull["T1409"] = "Access stored application data";
            techniquesFull["T1414"] = "Capture clipboard data";
            techniquesFull["T1412"] = "Capture SMS messages";
            techniquesFull["T1405"] = "Exploit TEE vulnerability";
            techniquesFull["T1417"] = "Input capture";
            techniquesFull["T1411"] = "Input prompt";
            techniquesFull["T1579"] = "Keychain";
            techniquesFull["T1410"] = "Network traffic capture or redirection";
            techniquesFull["T1416"] = "URI Hijacking";
            techniquesFull["T1418"] = "Application discovery";
            techniquesFull["T1523"] = "Evade analysis environment";
            techniquesFull["T1420"] = "File and directory discovery";
            techniquesFull["T1430"] = "Location tracking";
            techniquesFull["T1423"] = "Network service scanning";
            techniquesFull["T1424"] = "Process discovery";
            techniquesFull["T1426"] = "System infromation discovery";
            techniquesFull["T1422"] = "System netwrok configuration discovery";
            techniquesFull["T1421"] = "System network connections discovery";
            techniquesFull["T1427"] = "Attack PC via USB connection";
            techniquesFull["T1428"] = "Exploit enterprise resources";
            techniquesFull["T1435"] = "Access calendar entries";
            techniquesFull["T1433"] = "Access call log";
            techniquesFull["T1432"] = "Access contact list";
            techniquesFull["T1517"] = "Access notifications";
            techniquesFull["T1413"] = "Access sensitive data in device logs";
            techniquesFull["T1409"] = "Access stored application data";
            techniquesFull["T1429"] = "Capture audio";
            techniquesFull["T1512"] = "Capture camera";
            techniquesFull["T1414"] = "Capture clipboard data";
            techniquesFull["T1412"] = "Capture SMS messages";
            techniquesFull["T1533"] = "Data from local system";
            techniquesFull["T1541"] = "Foreground persistence";
            techniquesFull["T1417"] = "Input capture";
            techniquesFull["T1430"] = "Location tracking";
            techniquesFull["T1507"] = "Network information discovery";
            techniquesFull["T1410"] = "Network traffic capture or redirection";
            techniquesFull["T1513"] = "Screen capture";
            techniquesFull["T1438"] = "Alternate network mediums";
            techniquesFull["T1436"] = "Commonly used port";
            techniquesFull["T1520"] = "Domain generation algorithms";
            techniquesFull["T1544"] = "Remote file copy";
            techniquesFull["T1437"] = "Standard application layer protocol";
            techniquesFull["T1521"] = "Standard cryptographic protocol";
            techniquesFull["T1509"] = "Uncommonly used port";
            techniquesFull["T1481"] = "Web service";
            techniquesFull["T1438"] = "Alternate network mediums";
            techniquesFull["T1436"] = "Commonly used port";
            techniquesFull["T1532"] = "Data encrypted";
            techniquesFull["T1437"] = "Standard application layer protocol";
            techniquesFull["T1448"] = "Carrier billing fraud";
            techniquesFull["T1510"] = "Clipboard modification";
            techniquesFull["T1471"] = "Data encrypted for impact";
            techniquesFull["T1447"] = "Delete device data";
            techniquesFull["T1446"] = "Device lockout";
            techniquesFull["T1472"] = "Generate fraudulent advertising revenue";
            techniquesFull["T1516"] = "Input injection";
            techniquesFull["T1452"] = "Manipulate app store rankings or ratings";
            techniquesFull["T1400"] = "Modify system partition";
            techniquesFull["T1582"] = "SMS Control";
            techniquesFull["T1466"] = "Downgrade to insecure protocols";
            techniquesFull["T1439"] = "Eavesdrop on insecure network communication";
            techniquesFull["T1449"] = "Exploit SS7 to redirect phone calls/SMS";
            techniquesFull["T1450"] = "Exploit SS7 to track device location";
            techniquesFull["T1464"] = "Jamming or denial of service";
            techniquesFull["T1463"] = "Manipulate device communication";
            techniquesFull["T1467"] = "Rogue cellular base station";
            techniquesFull["T1465"] = "Rogue Wi-Fi access points";
            techniquesFull["T1451"] = "SIM card swap";
            techniquesFull["T1470"] = "Obtain device cloud backups";
            techniquesFull["T1468"] = "Remotely track device without authorization";
            techniquesFull["T1469"] = "Remotely wipe data without authorization";
            techniquesFull["T1615"] = "Group Policy Discovery";
            techniquesFull["T1616"] = "Call Control";
            techniquesFull["T1617"] = "Hooking";
            techniquesFull["T1618"] = "User Evasion";
            techniquesFull["T1619"] = "Cloud Storage Object Discovery";
            techniquesFull["T1620"] = "Reflective Code Loading";
            techniquesFull["T1547/015"] = "Boot or Logon Autostart Execution: Login Items";
            techniquesFull["ics-T0802"] = "Automated Collection";
            techniquesFull["ics-T0811"] = "Data from Information Repositories";
            techniquesFull["ics-T0868"] = "Detect Operating Mode";
            techniquesFull["ics-T0877"] = "I/O Image";
            techniquesFull["ics-T0830"] = "Man in the Middle";
            techniquesFull["ics-T0801"] = "Monitor Process State";
            techniquesFull["ics-T0861"] = "Point & Tag Identification";
            techniquesFull["ics-T0845"] = "Program Upload";
            techniquesFull["ics-T0852"] = "Screen Capture";
            techniquesFull["ics-T0887"] = "Wireless Sniffing";
            techniquesFull["ics-T0885"] = "Commonly Used Port";
            techniquesFull["ics-T0884"] = "Connection Proxy";
            techniquesFull["ics-T0869"] = "Standard Application Layer Protocol";
            techniquesFull["ics-T0840"] = "Network Connection Enumeration";
            techniquesFull["ics-T0842"] = "Network Sniffing";
            techniquesFull["ics-T0846"] = "Remote System Discovery";
            techniquesFull["ics-T0888"] = "Remote System Information Discovery";
            techniquesFull["ics-T0887"] = "Wireless Sniffing";
            techniquesFull["ics-T0858"] = "Change Operating Mode";
            techniquesFull["ics-T0820"] = "Exploitation for Evasion";
            techniquesFull["ics-T0872"] = "Indicator Removal on Host";
            techniquesFull["ics-T0849"] = "Masquerading";
            techniquesFull["ics-T0851"] = "Rootkit";
            techniquesFull["ics-T0856"] = "Spoof Reporting Message";
            techniquesFull["ics-T0858"] = "Change Operating Mode";
            techniquesFull["ics-T0807"] = "Command-Line Interface";
            techniquesFull["ics-T0871"] = "Execution through API";
            techniquesFull["ics-T0823"] = "Graphical User Interface";
            techniquesFull["ics-T0874"] = "Hooking";
            techniquesFull["ics-T0821"] = "Modify Controller Tasking";
            techniquesFull["ics-T0834"] = "Native API";
            techniquesFull["ics-T0853"] = "Scripting";
            techniquesFull["ics-T0863"] = "User Execution";
            techniquesFull["ics-T0879"] = "Damage to Property";
            techniquesFull["ics-T0813"] = "Denial of Control";
            techniquesFull["ics-T0815"] = "Denial of View";
            techniquesFull["ics-T0826"] = "Loss of Availability";
            techniquesFull["ics-T0827"] = "Loss of Control";
            techniquesFull["ics-T0828"] = "Loss of Productivity and Revenue";
            techniquesFull["ics-T0837"] = "Loss of Protection";
            techniquesFull["ics-T0880"] = "Loss of Safety";
            techniquesFull["ics-T0829"] = "Loss of View";
            techniquesFull["ics-T0831"] = "Manipulation of Control";
            techniquesFull["ics-T0832"] = "Manipulation of View";
            techniquesFull["ics-T0882"] = "Theft of Operational Information";
            techniquesFull["ics-T0806"] = "Brute Force I/O";
            techniquesFull["ics-T0836"] = "Modify Parameter";
            techniquesFull["ics-T0839"] = "Module Firmware";
            techniquesFull["ics-T0856"] = "Spoof Reporting Message";
            techniquesFull["ics-T0855"] = "Unauthorized Command Message";
            techniquesFull["ics-T0800"] = "Activate Firmware Update Mode";
            techniquesFull["ics-T0878"] = "Alarm Suppression";
            techniquesFull["ics-T0803"] = "Block Command Message";
            techniquesFull["ics-T0804"] = "Block Reporting Message";
            techniquesFull["ics-T0805"] = "Block Serial COM";
            techniquesFull["ics-T0809"] = "Data Destruction";
            techniquesFull["ics-T0814"] = "Denial of Service";
            techniquesFull["ics-T0816"] = "Device Restart/Shutdown";
            techniquesFull["ics-T0835"] = "Manipulate I/O Image";
            techniquesFull["ics-T0838"] = "Modify Alarm Settings";
            techniquesFull["ics-T0851"] = "Rootkit";
            techniquesFull["ics-T0881"] = "Service Stop";
            techniquesFull["ics-T0857"] = "System Firmware";
            techniquesFull["ics-T0810"] = "Data Historian Compromise";
            techniquesFull["ics-T0817"] = "Drive-by Compromise";
            techniquesFull["ics-T0818"] = "Engineering Workstation Compromise";
            techniquesFull["ics-T0819"] = "Exploit Public-Facing Application";
            techniquesFull["ics-T0866"] = "Exploitation of Remote Services";
            techniquesFull["ics-T0822"] = "External Remote Services";
            techniquesFull["ics-T0883"] = "Internet Accessible Device";
            techniquesFull["ics-T0886"] = "Remote Services";
            techniquesFull["ics-T0847"] = "Replication Through Removable Media";
            techniquesFull["ics-T0848"] = "Rogue Master";
            techniquesFull["ics-T0865"] = "Spearphishing Attachment";
            techniquesFull["ics-T0862"] = "Supply Chain Compromise";
            techniquesFull["ics-T0860"] = "Wireless Compromise";
            techniquesFull["ics-T0812"] = "Default Credentials";
            techniquesFull["ics-T0866"] = "Exploitation of Remote Services";
            techniquesFull["ics-T0867"] = "Lateral Tool Transfer";
            techniquesFull["ics-T0843"] = "Program Download";
            techniquesFull["ics-T0886"] = "Remote Services";
            techniquesFull["ics-T0859"] = "Valid Accounts";
            techniquesFull["ics-T0889"] = "Modify Program";
            techniquesFull["ics-T0839"] = "Module Firmware";
            techniquesFull["ics-T0873"] = "Project File Infection";
            techniquesFull["ics-T0857"] = "System Firmware";
            techniquesFull["ics-T0859"] = "Valid Accounts";
            techniquesFull["ics-T0890"] = "Exploitation for Privilege Escalation";
            techniquesFull["ics-T0874"] = "Hooking";
            #endregion

            //start off our htmlContent
            htmlContent = new List<string>();
            if (selected != "")
            {
                htmlContent.Add("<div style='padding-bottom:15px; padding-top:15px;'>Selected Technique: <span id='selectedTechniqueSpan'>" + selected + " - " + techniquesFull[selected] + "</span></div>");
            }
            else
            {
                htmlContent.Add("<div style='padding-bottom:15px; padding-top:15px;'>Selected Technique: <span id='selectedTechniqueSpan'></span></div>");
            }
            htmlContent.Add("<div id='accordion'>");

            // Populate our techniques list

            /* MITRE PRE HAS BEEN DEPRECATED 10-27-2020 */
            #region tactics/pre
            
            /*

            // tactics/pre -- count 15 at time of writing

            //htmlContent.Add("<optgroup label='PRE-ATT&CK'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark' id='heading-pre'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-pre'>MITRE ATT&CK PRE</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-pre' class='collapse' aria-labelledby='heading-pre' data-parent='#accordion'>");

            // tactics/pre/TA0012 -- count 13 at time of writing
            #region tactic TA0012
            techniques = new Dictionary<string, string>();
            techniques.Add("T1236", "Assess current holdings, needs, and wants");
            techniques.Add("T1229", "Assess KITs/KIQs benefits");
            techniques.Add("T1224", "Assess leadership areas of interest");
            techniques.Add("T1228", "Assign KITs/KIQs into categories");
            techniques.Add("T1226", "Conduct cost/benefit analysis");
            techniques.Add("T1232", "Create implementation plan");
            techniques.Add("T1231", "Create strategic plan");
            techniques.Add("T1230", "Derive intelligence requirements");
            techniques.Add("T1227", "Develop KITs/KIQs");
            techniques.Add("T1234", "Generate analyst intelligence requirements");
            techniques.Add("T1233", "Identify analyst level gaps");
            techniques.Add("T1225", "Identify gap areas");
            techniques.Add("T1235", "Receive operator KITs/KIQs tasking");

            // Add the TA0012->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0012'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0012'>TA0012 - Priority Definition Planning</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0012' class='collapse' aria-labelledby='heading-TA0012' data-parent='#collapse-pre'>");
            
            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            
            #endregion

            // tactics/pre/TA0013 -- count 4 at time of writing
            #region tactic TA0013
            techniques = new Dictionary<string, string>();
            techniques.Add("T1238", "Assign KITs, KIQs, and/or intelligence requirements");
            techniques.Add("T1239", "Receive KITs/KIQs and determine requirements");
            techniques.Add("T1237", "Submit KITs, KIQs and intelligence requirements");
            techniques.Add("T1240", "Task requirements");

            // Add the TA0013->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0013'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0013'>TA0013 - Priority Definition Direction</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0013' class='collapse' aria-labelledby='heading-TA0013' data-parent='#collapse-pre'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            #endregion

            // tactics/pre/TA0014 -- count 5 at time of writing
            #region tactic T0014
            techniques = new Dictionary<string, string>();
            techniques.Add("T1245", "Determine approach/attack vector");
            techniques.Add("T1243", "Determine highest level tactical element");
            techniques.Add("T1242", "Determine operational element");
            techniques.Add("T1244", "Determine secondary level tactical element");
            techniques.Add("T1241", "Determine strategic target");

            // Add the TA0014->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0014'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0014'>TA0014 - Target Selection</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0014' class='collapse' aria-labelledby='heading-TA0014' data-parent='#collapse-pre'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            #endregion

            // tactics/pre/TA0015 -- count 20 at time of writing
            #region tactic TA0015
            techniques = new Dictionary<string, string>();
            techniques.Add("T1247", "Acquire OSINT data sets and information");
            techniques.Add("T1254", "Conduct active scanning");
            techniques.Add("T1253", "Conduct passive scanning");
            techniques.Add("T1249", "Conduct social engineering");
            techniques.Add("T1260", "Determine 3rd party infrastructure services");
            techniques.Add("T1250", "Determine domain and IP address space");
            techniques.Add("T1259", "Determine external network trust dependencies");
            techniques.Add("T1258", "Determine firmware version");
            techniques.Add("T1255", "Discover target logon/email address format");
            techniques.Add("T1262", "Enumerate client configurations");
            techniques.Add("T1261", "Enumerate externally facing software applications technologies, languages, and dependencies");
            techniques.Add("T1248", "Identify job postings and needs/gaps");
            techniques.Add("T1263", "Identify security defensive capabilities");
            techniques.Add("T1246", "Identify supply chains");
            techniques.Add("T1264", "Identify technology usage patterns");
            techniques.Add("T1256", "Identify web defensive services");
            techniques.Add("T1252", "Map network topology");
            techniques.Add("T1257", "Mine technical blogs/forums");
            techniques.Add("T1251", "Obtain domain/IP registration information");
            techniques.Add("T1397", "Spearphishing for Information");

            // Add the TA0015->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0015'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0015'>TA0015 - Technical Information Gathering</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0015' class='collapse' aria-labelledby='heading-TA0015' data-parent='#collapse-pre'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            #endregion

            // tactics/pre/TA0016 -- count 11 at time of writing
            #region tactic T0016
            techniques = new Dictionary<string, string>();
            techniques.Add("T1266", "Acquire OSINT data sets and information");
            techniques.Add("T1275", "Aggregate individual's digital footprint");
            techniques.Add("T1268", "Conduct social engineering");
            techniques.Add("T1272", "Identify business relationships");
            techniques.Add("T1270", "Identify groups/roles");
            techniques.Add("T1267", "Identify job postings and needs/gaps");
            techniques.Add("T1269", "Identify people of interest");
            techniques.Add("T1271", "Identify personnel with an authority/privilege");
            techniques.Add("T1274", "Identify sensitive personnel information");
            techniques.Add("T1265", "Identify supply chains");
            techniques.Add("T1273", "Mine social media");

            // Add the TA0016->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0016'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0016'>TA0016 - People Information Gathering</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0016' class='collapse' aria-labelledby='heading-TA0016' data-parent='#collapse-pre'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            #endregion

            // tactics/pre/TA0017 -- count 11 at time of writing
            #region tactic T0017
            techniques = new Dictionary<string, string>();
            techniques.Add("T1277", "Acquire OSINT data sets and information");
            techniques.Add("T1279", "Conduct social engineering");
            techniques.Add("T1284", "Determine 3rd party infrastructure services");
            techniques.Add("T1285", "Determine centralization of IT management");
            techniques.Add("T1282", "Determine physical locations");
            techniques.Add("T1286", "Dumpster dive");
            techniques.Add("T1280", "Identify business processes/tempo");
            techniques.Add("T1283", "Identify business relationships");
            techniques.Add("T1278", "Identify job postings and needs/gaps");
            techniques.Add("T1276", "Identify supply chains");
            techniques.Add("T1281", "Obtain templates/branding materials");

            // Add the TA0017->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0017'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0017'>TA0017 - Organizational Information Gathering</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0017' class='collapse' aria-labelledby='heading-TA0017' data-parent='#collapse-pre'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            #endregion

            // tactics/pre/TA0018 -- count 9 at time of writing
            #region tactic T0018
            techniques = new Dictionary<string, string>();
            techniques.Add("T1293", "Analyze application security posture");
            techniques.Add("T1288", "Analyze architecture and configuration posture");
            techniques.Add("T1287", "Analyze data collected");
            techniques.Add("T1294", "Analyze hardware/software security defensive capabilities");
            techniques.Add("T1289", "Analyze organizational skillsets and deficiiencies");
            techniques.Add("T1389", "Identify vulnerabilities in third-party software libraries");
            techniques.Add("T1291", "Research relevant vulnerabilities/CVEs");
            techniques.Add("T1290", "Research visibility gap of security vendors");
            techniques.Add("T1292", "Test signature detection");

            // Add the TA0018->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0018'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0018'>TA0018 - Technical Weakness Identification</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0018' class='collapse' aria-labelledby='heading-TA0018' data-parent='#collapse-pre'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            #endregion

            // tactics/pre/TA0019 -- count 3 at time of writing
            #region tactic T0019
            techniques = new Dictionary<string, string>();
            techniques.Add("T1297", "Analyze organizational skillsets and deficiencies");
            techniques.Add("T1295", "Analyze social and business relationships, interests and affiliations");
            techniques.Add("T1296", "Assess targeting options");

            // Add the TA0019->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0019'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0019'>TA0019 - People Weakness Identification</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0019' class='collapse' aria-labelledby='heading-TA0019' data-parent='#collapse-pre'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            #endregion

            // tactics/pre/TA0020 -- count 6 at time of writing
            #region tactic TA0020
            techniques = new Dictionary<string, string>();
            techniques.Add("T1301", "Analyze business processes");
            techniques.Add("T1300", "Analyze organizational skillsets and deficiencies");
            techniques.Add("T1303", "Analyze presence of outsourced capabilities");
            techniques.Add("T1299", "Assess opportunities created by business deals");
            techniques.Add("T1302", "Assess security posture of physical locations");
            techniques.Add("T1298", "Assess vulnerability of 3rd party vendors");

            // Add the TA0020->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0020'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0020'>TA0020 - Organizational Weakness Identification</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0020' class='collapse' aria-labelledby='heading-TA0020' data-parent='#collapse-pre'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            #endregion

            // tactics/pre/TA0021 -- count 20 at time of writing
            #region tactic T0021
            techniques = new Dictionary<string, string>();
            techniques.Add("T1307", "Acquire and/or use 3rd party infrastructure services");
            techniques.Add("T1308", "Acquire and/or use 3rd party software services");
            techniques.Add("T1310", "Acquire or compromise 3rd party signing certificates");
            techniques.Add("T1306", "Anonymity services");
            techniques.Add("T1321", "Common, high volume protocols and software");
            techniques.Add("T1312", "Compromise 3rd party infrastructure to support delivery");
            techniques.Add("T1320", "Data Hiding");
            techniques.Add("T1311", "Dynamic DNS");
            techniques.Add("T1314", "Host-based hiding techniques");
            techniques.Add("T1322", "Misattributable credentials");
            techniques.Add("T1315", "Network-based hiding techniques");
            techniques.Add("T1316", "Non-traditional or less attributable payment options");
            techniques.Add("T1309", "Obfuscate infrastructure");
            techniques.Add("T1318", "Obfuscate operational infrastructure");
            techniques.Add("T1319", "Obfuscate or encrypt code");
            techniques.Add("T1313", "Obfuscation or cryptography");
            techniques.Add("T1390", "OS-vendor provided communication channels");
            techniques.Add("T1305", "Private whois services");
            techniques.Add("T1304", "Proxy/protocol relays");
            techniques.Add("T1317", "Secure and protect infrastructure");

            // Add the TA0021->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0021'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0021'>TA0021 - Adversary OPSEC</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0021' class='collapse' aria-labelledby='heading-TA0021' data-parent='#collapse-pre'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            #endregion

            // tactics/pre/TA0022 -- count 16 at time of writing
            #region tactic T0022
            techniques = new Dictionary<string, string>();
            techniques.Add("T1329", "Acquire and/or use 3rd party infrastructure services");
            techniques.Add("T1330", "Acquire and/or use 3rd party software services");
            techniques.Add("T1332", "Acquire or compromise 3rd party signing certificates");
            techniques.Add("T1328", "Buy domain name");
            techniques.Add("T1334", "Compromise 3rd party infrastructure to support delivery");
            techniques.Add("T1339", "Create backup infrastructure");
            techniques.Add("T1326", "Domain registration hijacking");
            techniques.Add("T1333", "Dynamic DNS");
            techniques.Add("T1336", "Install and configure hardware, network and systems");
            techniques.Add("T1331", "Obfuscate infrastructure");
            techniques.Add("T1396", "Obtain booter/stressor subscription");
            techniques.Add("T1335", "Procure required equipment and software");
            techniques.Add("T1340", "Shadow DNS");
            techniques.Add("T1337", "SSL certificate acquisition for domain");
            techniques.Add("T1338", "SSL certificate acquisition for trust breaking");
            techniques.Add("T1327", "Use multiple DNS infrastructure");

            // Add the TA0022->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0022'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0022'>TA0022 - Establish & Maintain Infrastructure</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0022' class='collapse' aria-labelledby='heading-TA0022' data-parent='#collapse-pre'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            #endregion

            // tactics/pre/TA0023 -- count 6 at time of writing
            #region tactic T0023
            techniques = new Dictionary<string, string>();
            techniques.Add("T1341", "Build social network persona");
            techniques.Add("T1391", "Choose pre-compromised mobile app developer account credentials or signing keys");
            techniques.Add("T1343", "Choose pre-compromised persona and affiliated accounts");
            techniques.Add("T1342", "Develop social network persona digital footprint");
            techniques.Add("T1344", "Friend/Follow/Connect to targets of interest");
            techniques.Add("T1392", "Obtain Apple iOS enterprise distribution key pair and certificate");

            // Add the TA0023->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0023'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0023'>TA0023 - Persona Development</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0023' class='collapse' aria-labelledby='heading-TA0023' data-parent='#collapse-pre'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            #endregion

            // tactics/pre/TA0024 -- count 11 at time of writing
            #region tactic TA0024
            techniques = new Dictionary<string, string>();
            techniques.Add("T1347", "Build and configure delivery systems");
            techniques.Add("T1349", "Build or acquire exploits");
            techniques.Add("T1352", "C2 protocol development");
            techniques.Add("T1354", "Compromise 3rd party or closed-source vulnerability/exploit infromation");
            techniques.Add("T1345", "Create custom payloads");
            techniques.Add("T1355", "Create infected removable media");
            techniques.Add("T1350", "Discover new exploitsand monitor exploit-provider forums");
            techniques.Add("T1348", "Identify resources required to build capabilities");
            techniques.Add("T1346", "Obtain/re-use payloads");
            techniques.Add("T1353", "Post compromise tool development");
            techniques.Add("T1351", "Remote access tool development");

            // Add the TA0024->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0024'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0024'>TA0024 - Build Capabilities</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0024' class='collapse' aria-labelledby='heading-TA0024' data-parent='#collapse-pre'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            #endregion

            // tactics/pre/TA0025 -- count 7 at time of writing
            #region tactic TA0025
            techniques = new Dictionary<string, string>();
            techniques.Add("T1358", "Review logs and residual traces");
            techniques.Add("T1393", "Test ability to evade automated mobile application security analysis performed by app stores");
            techniques.Add("T1356", "Test callback functionality");
            techniques.Add("T1357", "Test malware in various execution environments");
            techniques.Add("T1359", "Test malware to evade detection");
            techniques.Add("T1360", "Test physical access");
            techniques.Add("T1361", "Test signature detection for file upload/email filters");

            // Add the TA0025->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0025'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0025'>TA0025 - Test Capabilities</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0025' class='collapse' aria-labelledby='heading-TA0025' data-parent='#collapse-pre'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            #endregion

            // tactics/pre/TA0026 -- count 6 at time of writing
            #region tactic TA0026
            techniques = new Dictionary<string, string>();
            techniques.Add("T1379", "Disseminate removable media");
            techniques.Add("T1394", "Distribute malicious software development tools");
            techniques.Add("T1364", "Friend/Follow/Connect to targets of interest");
            techniques.Add("T1365", "Hardware of software supply chain implant");
            techniques.Add("T1363", "Port redirector");
            techniques.Add("T1362", "Upload, install and configure software/tools");

            // Add the TA0026->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0026'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0026'>TA0026 - Stage Capabilities</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0026' class='collapse' aria-labelledby='heading-TA0026' data-parent='#collapse-pre'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            #endregion

            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            */

            #endregion

            // tactics/enterprise -- count 14 at time of writing
            #region tactics/enterprise

            //htmlContent.Add("<optgroup label='Enterprise'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark' id='heading-enterprise'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-enterprise'>MITRE ATT&CK ENTERPRISE</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-enterprise' class='collapse' aria-labelledby='heading-enterprise' data-parent='#accordion'>");

            // tactics/enterprise/TA0043 -- count 41 at time of writing
            #region tactics TA0043
            techniques = new Dictionary<string, string>();
            techniques.Add("T1595", "Active Scanning");
            techniques.Add("T1595/001", "Active Scanning: Scanning IP Blocks");
            techniques.Add("T1595/002", "Active Scanning: Vulnerability Scanning");
            techniques.Add("T1592", "Gather Victim Host Information");
            techniques.Add("T1592/001", "Gather Victim Host Information: Hardware");
            techniques.Add("T1592/002", "Gather Victim Host Information: Software");
            techniques.Add("T1592/003", "Gather Victim Host Information: Firmware");
            techniques.Add("T1592/004", "Gather Victim Host Information: Client Configurations");
            techniques.Add("T1589", "Gather Victim Identity Information");
            techniques.Add("T1589/001", "Gather Victim Identity Information: Credentials");
            techniques.Add("T1589/002", "Gather Victim Identity Information: Email Addresses");
            techniques.Add("T1589/003", "Gather Victim Identity Information: Employee Names");
            techniques.Add("T1590", "Gather Victim Network Information");
            techniques.Add("T1590/001", "Gather Victim Network Information: Domain Properties");
            techniques.Add("T1590/002", "Gather Victim Network Information: DNS");
            techniques.Add("T1590/003", "Gather Victim Network Information: Network Trust Dependencies");
            techniques.Add("T1590/004", "Gather Victim Network Information: Network Topology");
            techniques.Add("T1590/005", "Gather Victim Network Information: IP Addresses");
            techniques.Add("T1590/006", "Gather Victim Network Information: Network Security Appliances");
            techniques.Add("T1591", "Gather Victim Org Information");
            techniques.Add("T1591/001", "Gather Victim Org Information: Determine Physical Locations");
            techniques.Add("T1591/002", "Gather Victim Org Information: Business Relationships");
            techniques.Add("T1591/003", "Gather Victim Org Information: Identify Business Tempo");
            techniques.Add("T1591/004", "Gather Victim Org Information: Identify Roles");
            techniques.Add("T1598", "Phishing for Information");
            techniques.Add("T1598/001", "Phishing for Information: Spearphishing Service");
            techniques.Add("T1598/002", "Phishing for Information: Spearphishing Attachment");
            techniques.Add("T1598/003", "Phishing for Information: Spearphishing Link");
            techniques.Add("T1597", "Search Closed Sources");
            techniques.Add("T1597/001", "Search Closed Sources: Threat Intel Vendors");
            techniques.Add("T1597/002", "Search Closed Sources: Purchase Technical Data");
            techniques.Add("T1596", "Search Open Technical Databases");
            techniques.Add("T1596/001", "Search Open Technical Databases: DNS/Passive DNS");
            techniques.Add("T1596/002", "Search Open Technical Databases: WHOIS");
            techniques.Add("T1596/003", "Search Open Technical Databases: Digital Certificates");
            techniques.Add("T1596/004", "Search Open Technical Databases: CDNs");
            techniques.Add("T1596/005", "Search Open Technical Databases: Scan Databases");
            techniques.Add("T1593", "Search Open Websites/Domains");
            techniques.Add("T1593/001", "Search Open Websites/Domains: Social Media");
            techniques.Add("T1593/002", "Search Open Websites/Domains: Search Engines");
            techniques.Add("T1594", "Search Victim-Owned Websites");

            // Add the TA0043->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0043'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0043'>TA0043 - Reconnaissance</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0043' class='collapse' aria-labelledby='heading-TA0043' data-parent='#collapse-enterprise'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            #endregion

            // tactics/enterprise/TA0042 -- count 32 at time of writing
            #region tactics TA0042
            techniques = new Dictionary<string, string>();
            techniques.Add("T1583", "Acquire Infrastructure");
            techniques.Add("T1583/001", "Acquire Infrastructure: Domains");
            techniques.Add("T1583/002", "Acquire Infrastructure: DNS Server");
            techniques.Add("T1583/003", "Acquire Infrastructure: Virtual Private Server");
            techniques.Add("T1583/004", "Acquire Infrastructure: Server");
            techniques.Add("T1583/005", "Acquire Infrastructure: Botnet");
            techniques.Add("T1583/006", "Acquire Infrastructure: Web Services");
            techniques.Add("T1586", "Compromise Accounts");
            techniques.Add("T1586/001", "Compromise Accounts: Social Media Accounts");
            techniques.Add("T1586/002", "Compromise Accounts: Email Accounts");
            techniques.Add("T1584", "Compromise Infrastructure");
            techniques.Add("T1584/001", "Compromise Infrastructure: Domains");
            techniques.Add("T1584/002", "Compromise Infrastructure: DNS Server");
            techniques.Add("T1584/003", "Compromise Infrastructure: Virtual Private Server");
            techniques.Add("T1584/004", "Compromise Infrastructure: Server");
            techniques.Add("T1584/005", "Compromise Infrastructure: Botnet");
            techniques.Add("T1584/006", "Compromise Infrastructure: Web Services");
            techniques.Add("T1587", "Develop Capabilities");
            techniques.Add("T1587/001", "Develop Capabilities: Malware");
            techniques.Add("T1587/002", "Develop Capabilities: Code Signing Certificates");
            techniques.Add("T1587/003", "Develop Capabilities: Digital Certificates");
            techniques.Add("T1587/004", "Develop Capabilities: Exploits");
            techniques.Add("T1585", "Establish Accounts");
            techniques.Add("T1585/001", "Establish Accounts: Social Media Accounts");
            techniques.Add("T1585/002", "Establish Accounts: Email Accounts");
            techniques.Add("T1588", "Obtain Capabilities");
            techniques.Add("T1588/001", "Obtain Capabilities: Malware");
            techniques.Add("T1588/002", "Obtain Capabilities: Tool");
            techniques.Add("T1588/003", "Obtain Capabilities: Code Signing Certificates");
            techniques.Add("T1588/004", "Obtain Capabilities: Digital Certificates");
            techniques.Add("T1588/005", "Obtain Capabilities: Exploits");
            techniques.Add("T1588/006", "Obtain Capabilities: Vulnerabilities");
            techniques.Add("T1608", "Stage Capabilities");
            techniques.Add("T1608/001", "Stage Capabilities: Upload Malware");
            techniques.Add("T1608/002", "Stage Capabilities: Upload Tool");
            techniques.Add("T1608/003", "Stage Capabilities: Install Digital Certificate");
            techniques.Add("T1608/004", "Stage Capabilities: Drive-by Target");
            techniques.Add("T1608/005", "Stage Capabilities: Link Target");

            // Add the TA0042->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0042'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0042'>TA0042 - Resource Development</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0042' class='collapse' aria-labelledby='heading-TA0042' data-parent='#collapse-enterprise'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            #endregion

            // tactics/enterprise/TA0001 -- count 19 at time of writing
            #region tactics TA0001

            techniques = new Dictionary<string, string>();
            techniques.Add("T1189", "Drive-by Compromise");
            techniques.Add("T1190", "Exploit Public-Facing Application");
            techniques.Add("T1133", "External Remote Services");
            techniques.Add("T1200", "Hardware Additions");
            techniques.Add("T1566", "Phishing");
            techniques.Add("T1566/001", "Phishing: Spearphishing Attachment");
            techniques.Add("T1566/002", "Phishing: Spearphishing Link");
            techniques.Add("T1566/003", "Phishing: Spearphishing via Service");
            techniques.Add("T1091", "Replication Through Removable Media");
            techniques.Add("T1195", "Supply Chain Compromise");
            techniques.Add("T1195/001", "Supply Chain Compromise: Compromise Software Dependencies and Development Tools");
            techniques.Add("T1195/002", "Supply Chain Compromise: Compromise Software Supply Chain");
            techniques.Add("T1195/003", "Supply Chain Compromise: Compromise Hardware Supply Chain");
            techniques.Add("T1199", "Trusted Relationship");
            techniques.Add("T1078", "Valid Accounts");
            techniques.Add("T1078/001", "Valid Accounts: Default Accounts");
            techniques.Add("T1078/002", "Valid Accounts: Domain Accounts");
            techniques.Add("T1078/003", "Valid Accounts: Local Accounts");
            techniques.Add("T1078/004", "Valid Accounts: Cloud Accounts");

            // Add the TA0001->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0001'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0001'>TA0001 - Initial Access</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0001' class='collapse' aria-labelledby='heading-TA0001' data-parent='#collapse-enterprise'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            // tactics/enterprise/TA0002 -- count 30 at time of writing
            #region tactics TA0002

            techniques = new Dictionary<string, string>();
            techniques.Add("T1059", "Command and Scripting Interpreter");
            techniques.Add("T1059/001", "Command and Scripting Interpreter: PowerShell");
            techniques.Add("T1059/002", "Command and Scripting Interpreter: AppleScript");
            techniques.Add("T1059/003", "Command and Scripting Interpreter: Windows Command Shell");
            techniques.Add("T1059/004", "Command and Scripting Interpreter: Unix Shell");
            techniques.Add("T1059/005", "Command and Scripting Interpreter: Visual Basic");
            techniques.Add("T1059/006", "Command and Scripting Interpreter: Python");
            techniques.Add("T1059/007", "Command and Scripting Interpreter: JavaScript");
            techniques.Add("T1059/008", "Command and Scripting Interpreter: Network Device CLI");
            techniques.Add("T1609", "Container Administration Command");
            techniques.Add("T1610", "Deploy Container");
            techniques.Add("T1203", "Exploitation for Client Execution");
            techniques.Add("T1559", "Inter-Process Communication");
            techniques.Add("T1559/001", "Inter-Process Communication: Component Object Model");
            techniques.Add("T1559/002", "Inter-Process Communication: Dynamic Data Exchange");
            techniques.Add("T1106", "Native API");
            techniques.Add("T1053", "Scheduled Task/Job");
            techniques.Add("T1053/001", "Scheduled Task/Job: At (Linux)");
            techniques.Add("T1053/002", "Scheduled Task/Job: At (Windows)");
            techniques.Add("T1053/003", "Scheduled Task/Job: Cron");
            // techniques.Add("T1053/004", "Scheduled Task/Job: Launchd"); Deprecated v10
            techniques.Add("T1053/005", "Scheduled Task/Job: Scheduled Task");
            techniques.Add("T1053/006", "Scheduled Task/Job: Systemd Timers");
            techniques.Add("T1053/007", "Scheudled Task/Job: Container Orchestration Job");
            techniques.Add("T1129", "Shared Modules");
            techniques.Add("T1072", "Software Development Tools");
            techniques.Add("T1569", "System Services");
            techniques.Add("T1569/001", "System Services: Launchctl");
            techniques.Add("T1569/002", "System Services: Service Execution");
            techniques.Add("T1204", "User Execution");
            techniques.Add("T1204/001", "User Execution: Malicious Link");
            techniques.Add("T1204/002", "User Execution: Malicious File");
            techniques.Add("T1204/003", "User Execution: Malicious Image");
            techniques.Add("T1047", "Windows Management Instrumentation");

            // Add the TA0002->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0002'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0002'>TA0002 - Execution</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0002' class='collapse' aria-labelledby='heading-TA0002' data-parent='#collapse-enterprise'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            // tactics/enterprise/TA0003 -- count 97 at time of writing
            #region tactics TA0003

            techniques = new Dictionary<string, string>();
            techniques.Add("T1098", "Account Manipulation");
            techniques.Add("T1098/001", "Account Manipulation: Additional Cloud Credentials");
            techniques.Add("T1098/002", "Account Manipulation: Exchange Email Delegate Permissions");
            techniques.Add("T1098/003", "Account Manipulation: Add Office 365 Global Administrator Role");
            techniques.Add("T1098/004", "Account Manipulation: SSH Authorized Keys");
            techniques.Add("T1197", "BITS Jobs");
            techniques.Add("T1547", "Boot or Logon Autostart Execution");
            techniques.Add("T1547/001", "Boot or Logon Autostart Execution: Registry Run Keys / Startup Folder");
            techniques.Add("T1547/002", "Boot or Logon Autostart Execution: Authentication Package");
            techniques.Add("T1547/003", "Boot or Logon Autostart Execution: Time Providers");
            techniques.Add("T1547/004", "Boot or Logon Autostart Execution: Winlogon Helper DLL");
            techniques.Add("T1547/005", "Boot or Logon Autostart Execution: Security Support Provider");
            techniques.Add("T1547/006", "Boot or Logon Autostart Execution: Kernel Modules and Extensions");
            techniques.Add("T1547/007", "Boot or Logon Autostart Execution: Re-opened Applications");
            techniques.Add("T1547/008", "Boot or Logon Autostart Execution: LSASS Driver");
            techniques.Add("T1547/009", "Boot or Logon Autostart Execution: Shortcut Modification");
            techniques.Add("T1547/010", "Boot or Logon Autostart Execution: Port Monitors");
            techniques.Add("T1547/011", "Boot or Logon Autostart Execution: Plist Modification");
            techniques.Add("T1547/012", "Boot or Logon Autostart Execution: Print Processors");
            techniques.Add("T1547/013", "Boot or Logon Autostart Execution: XDG Autostart Entries");
            techniques.Add("T1547/014", "Boot or Logon Autostart Execution: Active Setup");
            techniques.Add("T1547/015", "Boot or Logon Autostart Execution: Login Items");
            techniques.Add("T1037", "Boot or Logon Initialization Scripts");
            techniques.Add("T1037/001", "Boot or Logon Initialization Scripts: Logon Script (Windows)");
            techniques.Add("T1037/002", "Boot or Logon Initialization Scripts: Logon Script (Linux)");
            techniques.Add("T1037/003", "Boot or Logon Initialization Scripts: Network Logon Script");
            techniques.Add("T1037/004", "Boot or Logon Initialization Scripts: RC Scripts");
            techniques.Add("T1037/005", "Boot or Logon Initialization Scripts: Startup Items");
            techniques.Add("T1176", "Browser Extensions");
            techniques.Add("T1554", "Compromise Client Software Binary");
            techniques.Add("T1136", "Create Account");
            techniques.Add("T1136/001", "Create Account: Local Account");
            techniques.Add("T1136/002", "Create Account: Domain Account");
            techniques.Add("T1136/003", "Create Account: Cloud Account");
            techniques.Add("T1543", "Create or Modify System Process");
            techniques.Add("T1543/001", "Create or Modify System Process: Launch Agent");
            techniques.Add("T1543/002", "Create or Modify System Process: Systemd Service");
            techniques.Add("T1543/003", "Create or Modify System Process: Windows Service");
            techniques.Add("T1543/004", "Create or Modify System Process: Launch Daemon");
            techniques.Add("T1546", "Event Triggered Execution");
            techniques.Add("T1546/001", "Event Triggered Execution: Change default file association");
            techniques.Add("T1546/002", "Event Triggered Execution: Screensaver");
            techniques.Add("T1546/003", "Event Triggered Execution: Windows Management Instrumentation Event Subscription");
            techniques.Add("T1546/004", "Event Triggered Execution: Unix Shell Configuration Modification");
            techniques.Add("T1546/005", "Event Triggered Execution: Trap");
            techniques.Add("T1546/006", "Event Triggered Execution: LC_LOAD_DYLIB Addition");
            techniques.Add("T1546/007", "Event Triggered Execution: Netsh helper DLL");
            techniques.Add("T1546/008", "Event Triggered Execution: Accessibility features");
            techniques.Add("T1546/009", "Event Triggered Execution: AppCert DLLs");
            techniques.Add("T1546/010", "Event Triggered Execution: AppInit DLLs");
            techniques.Add("T1546/011", "Event Triggered Execution: Application Shimming");
            techniques.Add("T1546/012", "Event Triggered Execution: Image File Execution Options Injection");
            techniques.Add("T1546/013", "Event Triggered Execution: PowerShell profile");
            techniques.Add("T1546/014", "Event Triggered Execution: Emond");
            techniques.Add("T1546/015", "Event Triggered Execution: Component Object Model Hijacking");
            techniques.Add("T1133", "External Remote Services");
            techniques.Add("T1574", "Hijack Execution Flow");
            techniques.Add("T1574/001", "Hijack Execution Flow: DLL Search Order Hijacking");
            techniques.Add("T1574/002", "Hijack Execution Flow: DLL Side-Loading");
            techniques.Add("T1574/004", "Hijack Execution Flow: Dylib Hijacking");
            techniques.Add("T1574/005", "Hijack Execution Flow: Executable Installer File Permissions Weakness");
            techniques.Add("T1574/006", "Hijack Execution Flow: Dynamic Linker Hijacking");
            techniques.Add("T1574/007", "Hijack Execution Flow: Path Interception by PATH Environment Variable");
            techniques.Add("T1574/008", "Hijack Execution Flow: Path Interception by Search Order Hijacking");
            techniques.Add("T1574/009", "Hijack Execution Flow: Path Interception by Unquoted Path");
            techniques.Add("T1574/010", "Hijack Execution Flow: Services File Permissions Weakness");
            techniques.Add("T1574/011", "Hijack Execution Flow: Services Registry Permissions Weakness");
            techniques.Add("T1574/012", "Hijack Execution Flow: COR_PROFILER");
            techniques.Add("T1525", "Implant Internal Image");
            techniques.Add("T1556", "Modify Authentication Process");
            techniques.Add("T1556/001", "Modify Authentication Process: Domain Controller Authentication");
            techniques.Add("T1556/002", "Modify Authentication Process: Password Filter DLL");
            techniques.Add("T1556/003", "Modify Authentication Process: Pluggable Authentication Modules");
            techniques.Add("T1556/004", "Modify Authentication Process: Network Device Authentication");
            techniques.Add("T1137", "Office Application Startup");
            techniques.Add("T1137/001", "Office Application Startup: Office Template Macros");
            techniques.Add("T1137/002", "Office Application Startup: Office Test");
            techniques.Add("T1137/003", "Office Application Startup: Outlook Forms");
            techniques.Add("T1137/004", "Office Application Startup: Outlook Home Page");
            techniques.Add("T1137/005", "Office Application Startup: Outlook Rules");
            techniques.Add("T1137/006", "Office Application Startup: Add-ins");
            techniques.Add("T1542", "Pre-OS boot");
            techniques.Add("T1542/001", "Pre-OS boot: System firmware");
            techniques.Add("T1542/002", "Pre-OS boot: Component firmware");
            techniques.Add("T1542/003", "Pre-OS boot: Bootkit");
            techniques.Add("T1542/004", "Pre-OS boot: ROMMONkit");
            techniques.Add("T1542/005", "Pre-OS boot: TFTP Boot");
            techniques.Add("T1053", "Scheduled Task/Job");
            techniques.Add("T1053/001", "Scheduled Task/Job: At (Linux)");
            techniques.Add("T1053/002", "Scheduled Task/Job: At (Windows)");
            techniques.Add("T1053/003", "Scheduled Task/Job: Cron");
            //techniques.Add("T1053/004", "Scheduled Task/Job: Launchd"); Deprecated v10
            techniques.Add("T1053/005", "Scheduled Task/Job: Scheduled Task");
            techniques.Add("T1053/006", "Scheduled Task/Job: Systemd Timers");
            techniques.Add("T1053/007", "Scheduled Task/Job: Container Orchestration Job");
            techniques.Add("T1505", "Server Software Component");
            techniques.Add("T1505/001", "Server Software Component: SQL Stored Procedures");
            techniques.Add("T1505/002", "Server Software Component: Transport Agent");
            techniques.Add("T1505/003", "Server Software Component: Web Shell");
            techniques.Add("T1505/004", "Server Software Component: IIS Components");
            techniques.Add("T1205", "Traffic Signaling");
            techniques.Add("T1205/001", "Traffic Signaling: Port Knocking");
            techniques.Add("T1078", "Valid Accounts");
            techniques.Add("T1078/001", "Valid Accounts: Default Accounts");
            techniques.Add("T1078/002", "Valid Accounts: Domain Accounts");
            techniques.Add("T1078/003", "Valid Accounts: Local Accounts");
            techniques.Add("T1078/004", "Valid Accounts: Cloud Accounts");
            
            // Add the TA0003->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0003'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0003'>TA0003 - Persistence</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0003' class='collapse' aria-labelledby='heading-TA0003' data-parent='#collapse-enterprise'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            // tactics/enterprise/TA0004 -- count 89 at time of writing
            #region tactics TA0004

            techniques = new Dictionary<string, string>();
            techniques.Add("T1548", "Abuse elevation control mechanism");
            techniques.Add("T1548/001", "Abuse elevation control mechanism: Setuid and Setgid");
            techniques.Add("T1548/002", "Abuse elevation control mechanism: Bypass user access control");
            techniques.Add("T1548/003", "Abuse elevation control mechanism: Sudo and sudo control");
            techniques.Add("T1548/004", "Abuse elevation control mechanism: Elevated execution with prompt");
            techniques.Add("T1134", "Access Token Manipulation");
            techniques.Add("T1134/001", "Access Token Manipulation: Token Impersonation/Theft");
            techniques.Add("T1134/002", "Access Token Manipulation: Create Process with Token");
            techniques.Add("T1134/003", "Access Token Manipulation: Make and Impersonate Token");
            techniques.Add("T1134/004", "Access Token Manipulation: Parent PID Spoofing");
            techniques.Add("T1134/005", "Access Token Manipulation: SID-History Injection");
            techniques.Add("T1547", "Boot or Logon Autostart Execution");
            techniques.Add("T1547/001", "Boot or Logon Autostart Execution: Registry Run Keys / Startup Folder");
            techniques.Add("T1547/002", "Boot or Logon Autostart Execution: Authenticaton Package");
            techniques.Add("T1547/003", "Boot or Logon Autostart Execution: Time Providers");
            techniques.Add("T1547/004", "Boot or Logon Autostart Execution: Winlogon Helper DLL");
            techniques.Add("T1547/005", "Boot or Logon Autostart Execution: Security Support Provider");
            techniques.Add("T1547/006", "Boot or Logon Autostart Execution: Kernel Modules and Extensions");
            techniques.Add("T1547/007", "Boot or Logon Autostart Execution: Re-opened Applications");
            techniques.Add("T1547/008", "Boot or Logon Autostart Execution: LSASS Driver");
            techniques.Add("T1547/009", "Boot or Logon Autostart Execution: Shortcut Modification");
            techniques.Add("T1547/010", "Boot or Logon Autostart Execution: Port Monitors");
            techniques.Add("T1547/011", "Boot or Logon Autostart Execution: Plist Modification");
            techniques.Add("T1547/012", "Boot or Logon Autostart Execution: Print Processors");
            techniques.Add("T1547/013", "Boot or Logon Autostart Execution: XDG Autostart Entries");
            techniques.Add("T1547/014", "Boot or Logon Autostart Execution: Active Setup");
            techniques.Add("T1547/015", "Boot or Logon Autostart Execution: Login Items");
            techniques.Add("T1037", "Boot or Logon Initialization Scripts");
            techniques.Add("T1037/001", "Boot or Logon Initialization Scripts: Logon Script (Windows)");
            techniques.Add("T1037/002", "Boot or Logon Initialization Scripts: Logon Script (Linux)");
            techniques.Add("T1037/003", "Boot or Logon Initialization Scripts: Network Logon Script");
            techniques.Add("T1037/004", "Boot or Logon Initialization Scripts: RC Scrits");
            techniques.Add("T1037/005", "Boot or Logon Initialization Scripts: Startup Items");
            techniques.Add("T1543", "Create or Modify System Process");
            techniques.Add("T1543/001", "Create or Modify System Process: Launch Agent");
            techniques.Add("T1543/002", "Create or Modify System Process: Systemd Service");
            techniques.Add("T1543/003", "Create or Modify System Process: Windows Service");
            techniques.Add("T1543/004", "Create or Modify System Process: Launch Daemon");
            techniques.Add("T1484", "Domain Policy Modification");
            techniques.Add("T1484/001", "Domain Policy Modification: Group Policy Modification");
            techniques.Add("T1484/002", "Domain Policy Modification: Domain Trust Modification");
            techniques.Add("T1611", "Escape to Host");
            techniques.Add("T1546", "Event Triggered Execution");
            techniques.Add("T1546/001", "Event Triggered Execution: Change default file association");
            techniques.Add("T1546/002", "Event Triggered Execution: Screensaver");
            techniques.Add("T1546/003", "Event Triggered Execution: Windows Management Instrumentation Event Subscription");
            techniques.Add("T1546/004", "Event Triggered Execution: Unix Shell Configuration Modification");
            techniques.Add("T1546/005", "Event Triggered Execution: Trap");
            techniques.Add("T1546/006", "Event Triggered Execution: LC_LOAD_DYLIB Addition");
            techniques.Add("T1546/007", "Event Triggered Execution: Netsh helper DLL");
            techniques.Add("T1546/008", "Event Triggered Execution: Accessibility features");
            techniques.Add("T1546/009", "Event Triggered Execution: AppCert DLLs");
            techniques.Add("T1546/010", "Event Triggered Execution: AppInit DLLs");
            techniques.Add("T1546/011", "Event Triggered Execution: Application Shimming");
            techniques.Add("T1546/012", "Event Triggered Execution: Image File Execution Options Injection");
            techniques.Add("T1546/013", "Event Triggered Execution: PowerShell profile");
            techniques.Add("T1546/014", "Event Triggered Execution: Emond");
            techniques.Add("T1546/015", "Event Triggered Execution: Component Object Model Hijacking");
            techniques.Add("T1068", "Exploitation for Privilege Escalation");
            //techniques.Add("T1484", "Group policy modification");
            techniques.Add("T1574", "Hijack Execution Flow");
            techniques.Add("T1574/001", "Hijack Execution Flow: DLL Search Order Hijacking");
            techniques.Add("T1574/002", "Hijack Execution Flow: DLL Side-Loading");
            techniques.Add("T1574/004", "Hijack Execution Flow: Dylib Hijacking");
            techniques.Add("T1574/005", "Hijack Execution Flow: Executable Installer File Permissions Weakness");
            techniques.Add("T1574/006", "Hijack Execution Flow: Dynamic Linker Hijacking");
            techniques.Add("T1574/007", "Hijack Execution Flow: Path Interception by PATH Environment Variable");
            techniques.Add("T1574/008", "Hijack Execution Flow: Path Interception by Search Order Hijacking");
            techniques.Add("T1574/009", "Hijack Execution Flow: Path Interception by Unquoted Path");
            techniques.Add("T1574/010", "Hijack Execution Flow: Services File Permissions Weakness");
            techniques.Add("T1574/011", "Hijack Execution Flow: Services Registry Permissions Weakness");
            techniques.Add("T1574/012", "Hijack Execution Flow: COR_PROFILER");
            techniques.Add("T1055", "Process Injection");
            techniques.Add("T1055/001", "Process Injection: Dynamic-link Library Injection");
            techniques.Add("T1055/002", "Process Injection: Portable Executable Injection");
            techniques.Add("T1055/003", "Process Injection: Thread Execution Hijacking");
            techniques.Add("T1055/004", "Process Injection: Asynchronous Procedure Call");
            techniques.Add("T1055/005", "Process Injection: Thread Local Storage");
            techniques.Add("T1055/008", "Process Injection: Ptrace System Calls");
            techniques.Add("T1055/009", "Process Injection: Proc Memory");
            techniques.Add("T1055/011", "Process Injection: Extra Window Memory Injection");
            techniques.Add("T1055/012", "Process Injection: Process Hollowing");
            techniques.Add("T1055/013", "Process Injection: Process Doppelganging");
            techniques.Add("T1055/014", "Process Injection: VDSO Hijacking");
            techniques.Add("T1053", "Scheduled Task/Job");
            techniques.Add("T1053/001", "Scheduled Task/Job: At (Linux)");
            techniques.Add("T1053/002", "Scheduled Task/Job: At (Windows)");
            techniques.Add("T1053/003", "Scheduled Task/Job: Cron");
            //techniques.Add("T1053/004", "Scheduled Task/Job: Launchd"); Deprecated v10
            techniques.Add("T1053/005", "Scheduled Task/Job: Scheduled Task");
            techniques.Add("T1053/006", "Scheduled Task/Job: Systemd Timers");
            techniques.Add("T1053/007", "Scheduled Task/Job: Container Orchestration Job");
            techniques.Add("T1078", "Valid Accounts");
            techniques.Add("T1078/001", "Valid Accounts: Default Accounts");
            techniques.Add("T1078/002", "Valid Accounts: Domain Accounts");
            techniques.Add("T1078/003", "Valid Accounts: Local Accounts");
            techniques.Add("T1078/004", "Valid Accounts: Cloud Accounts");

            // Add the TA0004->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0004'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0004'>TA0004 - Privilege Escalation</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0004' class='collapse' aria-labelledby='heading-TA0004' data-parent='#collapse-enterprise'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            // tactics/enterprise/TA0005 -- count 149 at time of writing
            #region tactics TA0005

            techniques = new Dictionary<string, string>();
            techniques.Add("T1548", "Abuse elevation control mechanism");
            techniques.Add("T1548/001", "Abuse elevation control mechanism: Setuid and setgid");
            techniques.Add("T1548/002", "Abuse elevation control mechanism: Bypass user access control");
            techniques.Add("T1548/003", "Abuse elevation control mechanism: Sudo and sudo caching");
            techniques.Add("T1548/004", "Abuse elevation control mechanism: Elevated execution with prompt");
            techniques.Add("T1134", "Access Token Manipulation");
            techniques.Add("T1134/001", "Access Token Manipulation: Token Impersonation/Theft");
            techniques.Add("T1134/002", "Access Token Manipulation: Create Process with Token");
            techniques.Add("T1134/003", "Access Token Manipulation: Make and Impersonate Token");
            techniques.Add("T1134/004", "Access Token Manipulation: Parent PID Spoofing");
            techniques.Add("T1134/005", "Access Token Manipulation: SID-History Injection");
            techniques.Add("T1197", "BITS Jobs");
            techniques.Add("T1610", "Deploy Container");
            techniques.Add("T1612", "Build Image on Host");
            techniques.Add("T1140", "Deobfuscate/decode files or information");
            techniques.Add("T1006", "Direct Volume Access");
            techniques.Add("T1480", "Execution Guardrails");
            techniques.Add("T1480/001", "Execution Guardrails: Environmental Keying");
            techniques.Add("T1211", "Exploitation for defense evasion");
            techniques.Add("T1222", "File and Directory Permissions Modificaton");
            techniques.Add("T1222/001", "File and Directory Permissions Modificaton: Windows File and Directory Permissions Modification");
            techniques.Add("T1222/002", "File and Directory Permissions Modificaton: Linux and Mac File and Directory Permissions Modification");
            //techniques.Add("T1484", "Group policy modification");
            techniques.Add("T1564", "Hide Artifacts");
            techniques.Add("T1564/001", "Hide Artifacts: Hidden Files and Directories");
            techniques.Add("T1564/002", "Hide Artifacts: Hidden Users");
            techniques.Add("T1564/003", "Hide Artifacts: Hidden Window");
            techniques.Add("T1564/004", "Hide Artifacts: NTFS File Attributes");
            techniques.Add("T1564/005", "Hide Artifacts: Hidden File System");
            techniques.Add("T1564/006", "Hide Artifacts: Run Virtual Instance");
            techniques.Add("T1564/007", "Hide Artifacts: VBA Stomping");
            techniques.Add("T1564/008", "Hide Artifacts: Email Hiding Rules");
            techniques.Add("T1564/009", "Hide Artifacts: Resource Forking");
            techniques.Add("T1574", "Hijack Execution Flow");
            techniques.Add("T1574/001", "Hijack Execution Flow: DLL Search Order Hijacking");
            techniques.Add("T1574/002", "Hijack Execution Flow: DLL Side-Loading");
            techniques.Add("T1574/004", "Hijack Execution Flow: Dylib Hijacking");
            techniques.Add("T1574/005", "Hijack Execution Flow: Executable Intaller File Permissions Weakness");
            techniques.Add("T1574/006", "Hijack Execution Flow: Dynamic Linker Hijacking");
            techniques.Add("T1574/007", "Hijack Execution Flow: Path Interception by PATH Environment Variable");
            techniques.Add("T1574/008", "Hijack Execution Flow: Path Interception by Search Order Hijacking");
            techniques.Add("T1574/009", "Hijack Execution Flow: Path Interception by Unquoted Path");
            techniques.Add("T1574/010", "Hijack Execution Flow: Services File Permissions Weakness");
            techniques.Add("T1574/011", "Hijack Execution Flow: Services Registry Permissions Weakness");
            techniques.Add("T1574/012", "Hijack Execution Flow: COR_PROFILER");
            techniques.Add("T1562", "Impair Defenses");
            techniques.Add("T1562/001", "Impair Defenses: Disable or Modify Tools");
            techniques.Add("T1562/002", "Impair Defenses: Disable Windows Event Logging");
            techniques.Add("T1562/003", "Impair Defenses: Impair Command History Logging");
            techniques.Add("T1562/004", "Impair Defenses: Disable or Modify System Firewall");
            techniques.Add("T1562/006", "Impair Defenses: Indicator Blocking");
            techniques.Add("T1562/007", "Impair Defenses: Disable or Modify Cloud Firewall");
            techniques.Add("T1562/008", "Impair Defenses: Disable Cloud Logs");
            techniques.Add("T1562/009", "Impair Defenses: Safe Mode Boot");
            techniques.Add("T1562/010", "Impair Defenses: Downgrade Attack");
            techniques.Add("T1070", "Indicator Removal on Host");
            techniques.Add("T1070/001", "Indicator Removal on Host: Clear Windows Event Logs");
            techniques.Add("T1070/002", "Indicator Removal on Host: Clear Linux or Mac System Logs");
            techniques.Add("T1070/003", "Indicator Removal on Host: Clear Command History");
            techniques.Add("T1070/004", "Indicator Removal on Host: File Deletion");
            techniques.Add("T1070/005", "Indicator Removal on Host: Network Share Connection Removal");
            techniques.Add("T1070/006", "Indicator Removal on Host: Timestomp");
            techniques.Add("T1202", "Indirect Command Execution");
            techniques.Add("T1036", "Masquerading");
            techniques.Add("T1036/001", "Masquerading: Invalid Code Signature");
            techniques.Add("T1036/002", "Masquerading: Right-to-Left Override");
            techniques.Add("T1036/003", "Masquerading: Rename System Utilities");
            techniques.Add("T1036/004", "Masquerading: Masquerade Task or Service");
            techniques.Add("T1036/005", "Masquerading: Match Legitimate Name or Location");
            techniques.Add("T1036/006", "Masquerading: Space after Filename");
            techniques.Add("T1036/007", "Masquerading: Double File Extension");
            techniques.Add("T1556", "Modify Authentication Process");
            techniques.Add("T1556/001", "Modify Authentication Process: Domain Controller Authentication");
            techniques.Add("T1556/002", "Modify Authentication Process: Password Filter DLL");
            techniques.Add("T1556/003", "Modify Authentication Process: Pluggable Authentication Modules");
            techniques.Add("T1556/004", "Modify Authentication Process: Network Device Authentication");
            techniques.Add("T1578", "Modify Cloud Compute Infrastructure");
            techniques.Add("T1578/001", "Modify Cloud Compute Infrastructure: Create Snapshot");
            techniques.Add("T1578/002", "Modify Cloud Compute Infrastructure: Create Cloud Instance");
            techniques.Add("T1578/003", "Modify Cloud Compute Infrastructure: Delete Cloud Instance");
            techniques.Add("T1578/004", "Modify Cloud Compute Infrastructure: Revert Cloud Instance");
            techniques.Add("T1112", "Modify Registry");
            techniques.Add("T1601", "Modify System Image");
            techniques.Add("T1601/001", "Modify System Image: Patch System Image");
            techniques.Add("T1601/002", "Modify System Image: Downgrade System Image");
            techniques.Add("T1599", "Network Boundary Bridging");
            techniques.Add("T1599/001", "Network Boundary Bridging: Network Address Translation Traversal");
            techniques.Add("T1027", "Obfuscated Files or Information");
            techniques.Add("T1027/001", "Obfuscated Files or Information: Binary Padding");
            techniques.Add("T1027/002", "Obfuscated Files or Information: Software Packing");
            techniques.Add("T1027/003", "Obfuscated Files or Information: Steganography");
            techniques.Add("T1027/004", "Obfuscated Files or Information: Compile after Delivery");
            techniques.Add("T1027/005", "Obfuscated Files or Information: Indicator Removal from Tools");
            techniques.Add("T1027/006", "Obfuscated Files or Information: HTML Smuggling");
            techniques.Add("T1542", "Pre-OS boot");
            techniques.Add("T1542/001", "Pre-OS boot: System Firmware");
            techniques.Add("T1542/002", "Pre-OS boot: Component Firmware");
            techniques.Add("T1542/003", "Pre-OS boot: Bootkit");
            techniques.Add("T1542/004", "Pre-OS boot: ROMMONkit");
            techniques.Add("T1542/005", "Pre-OS boot: TFTP Boot");
            techniques.Add("T1055", "Process Injection");
            techniques.Add("T1055/001", "Process Injection: Dynamic-link Library Injection");
            techniques.Add("T1055/002", "Process Injection: Portable Executable Injection");
            techniques.Add("T1055/003", "Process Injection: Thread Execution Hijacking");
            techniques.Add("T1055/004", "Process Injection: Asynchronous Procedure Call");
            techniques.Add("T1055/005", "Process Injection: Thread Local Storage");
            techniques.Add("T1055/008", "Process Injection: Ptrace System Calls");
            techniques.Add("T1055/009", "Process Injection: Proc Memory");
            techniques.Add("T1055/011", "Process Injection: Extra Window Memory Injection");
            techniques.Add("T1055/012", "Process Injection: Process Hollowing");
            techniques.Add("T1055/013", "Process Injection: Process Doppelganging");
            techniques.Add("T1055/014", "Process Injection: VDSO Hijacking");
            techniques.Add("T1207", "Rogue Domain Controller");
            techniques.Add("T1014", "Rootkit");
            techniques.Add("T1218", "Signed Binary Proxy Execution");
            techniques.Add("T1218/001", "Signed Binary Proxy Execution: Compiled HTML File");
            techniques.Add("T1218/002", "Signed Binary Proxy Execution: Control Panel");
            techniques.Add("T1218/003", "Signed Binary Proxy Execution: CMSTP");
            techniques.Add("T1218/004", "Signed Binary Proxy Execution: InstallUtil");
            techniques.Add("T1218/005", "Signed Binary Proxy Execution: Mshta");
            techniques.Add("T1218/007", "Signed Binary Proxy Execution: Msiexec");
            techniques.Add("T1218/008", "Signed Binary Proxy Execution: Odbcconf");
            techniques.Add("T1218/009", "Signed Binary Proxy Execution: Regsvcs/Regasm");
            techniques.Add("T1218/010", "Signed Binary Proxy Execution: Regsvr32");
            techniques.Add("T1218/011", "Signed Binary Proxy Execution: Rundll32");
            techniques.Add("T1218/012", "Signed Binary Proxy Execution: Verclsid");
            techniques.Add("T1218/013", "Signed Binary Proxy Execution: Mavinject");
            techniques.Add("T1218/014", "Signed Binary Proxy Execution: MMC");
            techniques.Add("T1216", "Signed Script Proxy Execution");
            techniques.Add("T1216/001", "Signed Script Proxy Execution: PubPrn");
            techniques.Add("T1553", "Subvert Trust Controls");
            techniques.Add("T1553/001", "Subvert Trust Controls: Gatekeeper Bypass");
            techniques.Add("T1553/002", "Subvert Trust Controls: Code Signing");
            techniques.Add("T1553/003", "Subvert Trust Controls: SIP and Trust Provider Hijacking");
            techniques.Add("T1553/004", "Subvert Trust Controls: Install Root Certificate");
            techniques.Add("T1553/005", "Subvert Trust Controls: Mark-of-the-Web Bypass");
            techniques.Add("T1553/006", "Subvert Trust Controls: Code Signing Policy Modificaiton");
            techniques.Add("T1221", "Template Injection");
            techniques.Add("T1205", "Traffic Signaling");
            techniques.Add("T1205/001", "Traffic Signaling: Port Knocking");
            techniques.Add("T1127", "Trusted Developer Utilities Proxy Execution");
            techniques.Add("T1127/001", "Trusted Developer Utilities Proxy Execution: MSBuild");
            techniques.Add("T1535", "Unused/Unsupported Cloud Regions");
            techniques.Add("T1550", "Use Alternative Authentication Material");
            techniques.Add("T1550/001", "Use Alternative Authentication Material: Application Access Token");
            techniques.Add("T1550/002", "Use Alternative Authentication Material: Pass the Hash");
            techniques.Add("T1550/003", "Use Alternative Authentication Material: Pass the Ticket");
            techniques.Add("T1550/004", "Use Alternative Authentication Material: Web Session Cookie");
            techniques.Add("T1078", "Valid Accounts");
            techniques.Add("T1078/001", "Valid Accounts: Default Accounts");
            techniques.Add("T1078/002", "Valid Accounts: Domain Accounts");
            techniques.Add("T1078/003", "Valid Accounts: Local Accounts");
            techniques.Add("T1078/004", "Valid Accounts: Cloud Accounts");
            techniques.Add("T1497", "Virtualization/Sandbox Evasion");
            techniques.Add("T1497/001", "Virtualization/Sandbox Evasion: System Checks");
            techniques.Add("T1497/002", "Virtualization/Sandbox Evasion: User Activity Based Checks");
            techniques.Add("T1497/003", "Virtualization/Sandbox Evasion: Time Based Evasion");
            techniques.Add("T1600", "Weaken Encryption");
            techniques.Add("T1600/001", "Weaken Encryption: Reduce Key Space");
            techniques.Add("T1600/002", "Weaken Encryption: Disable Crypto Hardware");
            techniques.Add("T1220", "XSL Script Processing");
            techniques.Add("T1484", "Domain Policy Modification");
            techniques.Add("T1484/001", "Domain Policy Modification: Group Policy Modification");
            techniques.Add("T1484/002", "Domain Policy Modification: Domain Trust Modification");
            techniques.Add("T1620", "Reflective Code Loading");

            // Add the TA0005->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0005'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0005'>TA0005 - Defense Evasion</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0005' class='collapse' aria-labelledby='heading-TA0005' data-parent='#collapse-enterprise'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            // tactics/enterprise/TA0006 -- count 49 at time of writing
            #region tactics TA0006

            techniques = new Dictionary<string, string>();
            techniques.Add("T1110", "Brute Force");
            techniques.Add("T1110/001", "Brute Force: Password Guessing");
            techniques.Add("T1110/002", "Brute Force: Password Cracking");
            techniques.Add("T1110/003", "Brute Force: Password Spraying");
            techniques.Add("T1110/004", "Brute Force: Credential Stuffing");
            techniques.Add("T1555", "Credentials from Password Stores");
            techniques.Add("T1555/001", "Credentials from Password Stores: Keychain");
            techniques.Add("T1555/002", "Credentials from Password Stores: Securityd Memory");
            techniques.Add("T1555/003", "Credentials from Password Stores: Credentials from Web Browsers");
            techniques.Add("T1555/004", "Credentials from Password Stores: Windows Credential Manager");
            techniques.Add("T1555/005", "Credentials from Password Stores: Password Managers");
            techniques.Add("T1212", "Exploitation for credential access");
            techniques.Add("T1187", "Forced authentication");
            techniques.Add("T1056", "Input Capture");
            techniques.Add("T1056/001", "Input Capture: Keylogging");
            techniques.Add("T1056/002", "Input Capture: GUI Input Capture");
            techniques.Add("T1056/003", "Input Capture: Web Portal Capture");
            techniques.Add("T1056/004", "Input Capture: Credential API Hooking");
            techniques.Add("T1557", "Adversary-in-the-Middle");
            techniques.Add("T1557/001", "Adversary-in-the-Middle: LLMNR/NBT-NS poisoning and SMB relay");
            techniques.Add("T1557/002", "Adversary-in-the-Middle: ARP Cache Poisoning");
            techniques.Add("T1556", "Modify Authentication Process");
            techniques.Add("T1556/001", "Modify Authentication Process: Domain Controller Authentication");
            techniques.Add("T1556/002", "Modify Authentication Process: Password Filter DLL");
            techniques.Add("T1556/003", "Modify Authentication Process: Pluggable Authentication Modules");
            techniques.Add("T1556/004", "Modify Authentication Process: Network Device Authentication");
            techniques.Add("T1040", "Network Sniffing");
            techniques.Add("T1003", "OS Credential Dumping");
            techniques.Add("T1003/001", "OS Credential Dumping: LSASS Memory");
            techniques.Add("T1003/002", "OS Credential Dumping: Security Account Manager");
            techniques.Add("T1003/003", "OS Credential Dumping: NTDS");
            techniques.Add("T1003/004", "OS Credential Dumping: LSA Secrets");
            techniques.Add("T1003/005", "OS Credential Dumping: Cached Domain Credentials");
            techniques.Add("T1003/006", "OS Credential Dumping: DCSync");
            techniques.Add("T1003/007", "OS Credential Dumping: Proc Filesystem");
            techniques.Add("T1003/008", "OS Credential Dumping: /etc/passwd and /etc/shadow");
            techniques.Add("T1528", "Steal Application Access Token");
            techniques.Add("T1558", "Steal or Forge Kerberos Tickets");
            techniques.Add("T1558/001", "Steal or Forge Kerberos Tickets: Golden Ticket");
            techniques.Add("T1558/002", "Steal or Forge Kerberos Tickets: Silver Ticket");
            techniques.Add("T1558/003", "Steal or Forge Kerberos Tickets: Kerberoasting");
            techniques.Add("T1558/004", "Steal or Forge Kerberos Tickets: AS-REP Roasting");
            techniques.Add("T1539", "Steal Web Session Cookie");
            techniques.Add("T1111", "Two-Factor Authentication Interception");
            techniques.Add("T1552", "Unsecured Credentials");
            techniques.Add("T1552/001", "Unsecured Credentials: Credentials in Files");
            techniques.Add("T1552/002", "Unsecured Credentials: Credentials in Registry");
            techniques.Add("T1552/003", "Unsecured Credentials: Bash History");
            techniques.Add("T1552/004", "Unsecured Credentials: Private Keys");
            techniques.Add("T1552/005", "Unsecured Credentials: Cloud Instance Metadata API");
            techniques.Add("T1552/006", "Unsecured Credentials: Group Policy Preferences");
            techniques.Add("T1552/007", "Unsecured Credentials: Container API");
            techniques.Add("T1606", "Forge Web Credentials");
            techniques.Add("T1606/001", "Forge Web Credentials: Web Cookies");
            techniques.Add("T1606/002", "Forge Web Credentials: SAML Tokens");

            // Add the TA0006->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0006'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0006'>TA0006 - Credential Access</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0006' class='collapse' aria-labelledby='heading-TA0006' data-parent='#collapse-enterprise'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            // tactics/enterprise/TA0007 -- count 36 at time of writing
            #region tactics TA0007

            techniques = new Dictionary<string, string>();
            techniques.Add("T1087", "Account Discovery");
            techniques.Add("T1087/001", "Account Discovery: Local Account");
            techniques.Add("T1087/002", "Account Discovery: Domain Account");
            techniques.Add("T1087/003", "Account Discovery: Email Account");
            techniques.Add("T1087/004", "Account Discovery: Cloud Account");
            techniques.Add("T1010", "Application Window Discovery");
            techniques.Add("T1217", "Browser Bookmark Discovery");
            techniques.Add("T1580", "Cloud Infrastructure Discovery");
            techniques.Add("T1538", "Cloud Service Dashboard");
            techniques.Add("T1526", "Cloud Service Discovery");
            techniques.Add("T1613", "Container and Resource Discovery");
            techniques.Add("T1482", "Domain Trust Discovery");
            techniques.Add("T1083", "File and Directory Discovery");
            techniques.Add("T1046", "Network Service Scanning");
            techniques.Add("T1135", "Network Share Discovery");
            techniques.Add("T1040", "Network Sniffing");
            techniques.Add("T1201", "Password Policy Discovery");
            techniques.Add("T1120", "Peripheral Device Discovery");
            techniques.Add("T1069", "Permission Groups Discovery");
            techniques.Add("T1069/001", "Permission Groups Discovery: Local Groups");
            techniques.Add("T1069/002", "Permission Groups Discovery: Domain Groups");
            techniques.Add("T1069/003", "Permission Groups Discovery: Cloud Groups");
            techniques.Add("T1057", "Process Discovery");
            techniques.Add("T1012", "Query Registry");
            techniques.Add("T1018", "Remote System Discovery");
            techniques.Add("T1518", "Software Discovery");
            techniques.Add("T1518/001", "Software Discovery: Security Software Discovery");
            techniques.Add("T1082", "System Information Discovery");
            techniques.Add("T1614", "System Location Discovery");
            techniques.Add("T1614/001", "System Location Discovery: System Language Discovery");
            techniques.Add("T1016", "System Network Configuration Discovery");
            techniques.Add("T1016/001", "System Network Configuration Discovery: Internet Connection Discovery");
            techniques.Add("T1049", "System Network Connections Discovery");
            techniques.Add("T1033", "System Owner/User Discovery");
            techniques.Add("T1007", "System Service Discovery");
            techniques.Add("T1124", "System Time Discovery");
            techniques.Add("T1497", "Virtualization/Sandbox Evasion");
            techniques.Add("T1497/001", "Virtualization/Sandbox Evasion: System Checks");
            techniques.Add("T1497/002", "Virtualization/Sandbox Evasion: User Activity Based Checks");
            techniques.Add("T1497/003", "Virtualization/Sandbox Evasion: Time Based Evasion");
            techniques.Add("T1619", "Cloud Storage Object Discovery");
            techniques.Add("T1615", "Group Policy Discovery");

            // Add the TA0007->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0007'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0007'>TA0007 - Discovery</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0007' class='collapse' aria-labelledby='heading-TA0007' data-parent='#collapse-enterprise'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            // tactics/enterprise/TA0008 -- count 21 at time of writing
            #region tactics TA0008

            techniques = new Dictionary<string, string>();
            techniques.Add("T1210", "Exploitation of Remote Services");
            techniques.Add("T1534", "Internal Spearphishing");
            techniques.Add("T1570", "Lateral Tool Transfer");
            techniques.Add("T1563", "Remote Service Session Hijacking");
            techniques.Add("T1563/001", "Remote Service Session Hijacking: SSH Hijacking");
            techniques.Add("T1563/002", "Remote Service Session Hijacking: RDP Hijacking");
            techniques.Add("T1021", "Remote Services");
            techniques.Add("T1021/001", "Remote Services: Remote Desktop Protocol");
            techniques.Add("T1021/002", "Remote Services: SMB/Windows Admin Shares");
            techniques.Add("T1021/003", "Remote Services: Distributed Component Object Model");
            techniques.Add("T1021/004", "Remote Services: SSH");
            techniques.Add("T1021/005", "Remote Services: VNC");
            techniques.Add("T1021/006", "Remote Services: Windows Remote Management");
            techniques.Add("T1091", "Replication Through Removable Media");
            techniques.Add("T1072", "Software Deployment Tools");
            techniques.Add("T1080", "Taint Shared Content");
            techniques.Add("T1550", "Use Alternate Authentication Material");
            techniques.Add("T1550/001", "Use Alternate Authentication Material: Application Access Token");
            techniques.Add("T1550/002", "Use Alternate Authentication Material: Pass the Hash");
            techniques.Add("T1550/003", "Use Alternate Authentication Material: Pass the Ticket");
            techniques.Add("T1550/004", "Use Alternate Authentication Material: Web Session Cookie");

            // Add the TA0008->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0008'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0008'>TA0008 - Lateral Movement</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0008' class='collapse' aria-labelledby='heading-TA0008' data-parent='#collapse-enterprise'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            // tactics/enterprise/TA0009 -- count 35 at time of writing
            #region tactics TA0009

            techniques = new Dictionary<string, string>();
            techniques.Add("T1560", "Archive Collected Data");
            techniques.Add("T1560/001", "Archive Collected Data: Archive via Utility");
            techniques.Add("T1560/002", "Archive Collected Data: Archive via Library");
            techniques.Add("T1560/003", "Archive Collected Data: Archive via Custom Method");
            techniques.Add("T1123", "Audio Capture");
            techniques.Add("T1119", "Automated Collection");
            techniques.Add("T1115", "Clipboard Data");
            techniques.Add("T1530", "Data from Cloud Storage Object");
            techniques.Add("T1602", "Data from Configuration Repository");
            techniques.Add("T1602/001", "Data from Configuration Repository: SNMP (MIB Dump)");
            techniques.Add("T1602/002", "Data from Configuration Repository: Network Device Configuration Dump");
            techniques.Add("T1213", "Data from Information Repositories");
            techniques.Add("T1213/001", "Data from Information Repositories: Confluence");
            techniques.Add("T1213/002", "Data from Information Repositories: Sharepoint");
            techniques.Add("T1213/003", "Data from Information Repositories: Code Repositories");
            techniques.Add("T1005", "Data from Local System");
            techniques.Add("T1039", "Data from Network Shared Drive");
            techniques.Add("T1025", "Data from Removable Media");
            techniques.Add("T1074", "Data Staged");
            techniques.Add("T1074/001", "Data Staged: Local Data Staging");
            techniques.Add("T1074/002", "Data Staged: Remote Data Staging");
            techniques.Add("T1114", "Email Collection");
            techniques.Add("T1114/001", "Email Collection: Local Email Collection");
            techniques.Add("T1114/002", "Email Collection: Remote Email Collection");
            techniques.Add("T1114/003", "Email Collection: Email Forwarding Rule");
            techniques.Add("T1056", "Input Capture");
            techniques.Add("T1056/001", "Input Capture: Keylogging");
            techniques.Add("T1056/002", "Input Capture: GUI input capture");
            techniques.Add("T1056/003", "Input Capture: Web portal capture");
            techniques.Add("T1056/004", "Input Capture: Credential API hooking");
            techniques.Add("T1185", "Browser Session Hijacking");
            techniques.Add("T1557", "Adversary-in-the-Middle");
            techniques.Add("T1557/001", "Adversary-in-the-Middle: LLMNR/NBT-NS Poisoning and SMB Relay");
            techniques.Add("T1557/002", "Adversary-in-the-Middle: ARP Cache Poisoning");
            techniques.Add("T1113", "Screen Capture");
            techniques.Add("T1125", "Video Capture");

            // Add the TA0009->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0009'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0009'>TA0009 - Collection</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0009' class='collapse' aria-labelledby='heading-TA0009' data-parent='#collapse-enterprise'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            // tactics/enterprise/TA0011 -- count 38 at time of writing
            #region tactics TA0011

            techniques = new Dictionary<string, string>();
            techniques.Add("T1071", "Application layer protocol");
            techniques.Add("T1071/001", "Application layer protocol: Web protocols");
            techniques.Add("T1071/002", "Application layer protocol: File transfer protocols");
            techniques.Add("T1071/003", "Application layer protocol: Mail protocols");
            techniques.Add("T1071/004", "Application layer protocol: DNS");
            techniques.Add("T1092", "Communication through removable media");
            techniques.Add("T1132", "Data encoding");
            techniques.Add("T1132/001", "Data encoding: Standard encoding");
            techniques.Add("T1132/002", "Data encoding: Non-standard encoding");
            techniques.Add("T1001", "Data obfuscation");
            techniques.Add("T1001/001", "Data obfuscation: Junk data");
            techniques.Add("T1001/002", "Data obfuscation: Steganography");
            techniques.Add("T1001/003", "Data obfuscation: Protocol impersonation");
            techniques.Add("T1568", "Dynamic resolution");
            techniques.Add("T1568/001", "Dynamic resolution: Fast Flux DNS");
            techniques.Add("T1568/002", "Dynamic resolution: Domain Generation Algorithms");
            techniques.Add("T1568/003", "Dynamic resolution: DNS Calculation");
            techniques.Add("T1573", "Encrypted Channel");
            techniques.Add("T1573/001", "Encrypted Channel: Symmetric Cryptography");
            techniques.Add("T1573/002", "Encrypted Channel: Asymmetric cryptography");
            techniques.Add("T1008", "Fallback channels");
            techniques.Add("T1105", "Ingress tool transfer");
            techniques.Add("T1104", "Multi-stage channels");
            techniques.Add("T1095", "Non-application layer protocol");
            techniques.Add("T1571", "Non-standard port");
            techniques.Add("T1572", "Protocol tunneling");
            techniques.Add("T1090", "Proxy");
            techniques.Add("T1090/001", "Proxy: Internal proxy");
            techniques.Add("T1090/002", "Proxy: External proxy");
            techniques.Add("T1090/003", "Proxy: Multi-hop proxy");
            techniques.Add("T1090/004", "Proxy: Domain fronting");
            techniques.Add("T1219", "Remote Access Software");
            techniques.Add("T1205", "Traffic Signaling");
            techniques.Add("T1205/001", "Traffic Signaling: Port Knocking");
            techniques.Add("T1102", "Web service");
            techniques.Add("T1102/001", "Web service: Dead drop resolver");
            techniques.Add("T1102/002", "Web service: Bidirectional communication");
            techniques.Add("T1102/003", "Web service: One-way communication");

            // Add the TA0011->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0011'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0011'>TA0011 - Command and Control</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0011' class='collapse' aria-labelledby='heading-TA0011' data-parent='#collapse-enterprise'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            // tactics/enterprise/TA0010 -- count 17 at time of writing
            #region tactics TA0010

            techniques = new Dictionary<string, string>();
            techniques.Add("T1020", "Automated Exfiltration");
            techniques.Add("T1020/001", "Automated Exfiltration: Traffic Duplication");
            techniques.Add("T1030", "Data trasnfer size limits");
            techniques.Add("T1048", "Exfiltration Over Alternative Protocol");
            techniques.Add("T1048/001", "Exfiltration Over Alternative Protocol: Exfiltration Over Symmetric Encrypted Non-C2 Protocol");
            techniques.Add("T1048/002", "Exfiltration Over Alternative Protocol: Exfiltration Over Asymmetric Encrypted Non-C2 Protocol");
            techniques.Add("T1048/003", "Exfiltration Over Alternative Protocol: Exfiltration Over Unencrypted/Obfuscated Non-C2 Protocol");
            techniques.Add("T1041", "Exfiltration Over C2 Channel");
            techniques.Add("T1011", "Exfiltration over other network medium");
            techniques.Add("T1011/001", "Exfiltration over other network medium: Exfiltration over bluetooth");
            techniques.Add("T1052", "Exfiltration Over Physical Medium");
            techniques.Add("T1052/001", "Exfiltration Over Physical Medium: Exfiltration Over USB");
            techniques.Add("T1567", "Exfiltration Over Web Service");
            techniques.Add("T1567/001", "Exfiltration Over Web Service: Exfiltration to Code Repository");
            techniques.Add("T1567/002", "Exfiltration Over Web Service: Exfiltration to Cloud Storage");
            techniques.Add("T1029", "Scheduled transfer");
            techniques.Add("T1537", "Transfer Data to Cloud Account");

            // Add the TA0010->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0010'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0010'>TA0010 - Exfiltration</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0010' class='collapse' aria-labelledby='heading-TA0010' data-parent='#collapse-enterprise'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            // tactics/enterprise/TA0040 -- count 26 at time of writing
            #region tactics TA0040

            techniques = new Dictionary<string, string>();
            techniques.Add("T1531", "Account Access Removal");
            techniques.Add("T1485", "Data Destruction");
            techniques.Add("T1486", "Data Encrypted for Impact");
            techniques.Add("T1565", "Data Manipulation");
            techniques.Add("T1565/001", "Data Manipulation: Stored Data Manipulation");
            techniques.Add("T1565/002", "Data Manipulation: Transmitted Data Manipulation");
            techniques.Add("T1565/003", "Data Manipulation: Runtime Data Manipulation");
            techniques.Add("T1491", "Defacement");
            techniques.Add("T1491/001", "Defacement: Internal Defacement");
            techniques.Add("T1491/002", "Defacement: External Defacement");
            techniques.Add("T1561", "Disk Wipe");
            techniques.Add("T1561/001", "Disk Wipe: Disk Content Wipe");
            techniques.Add("T1561/002", "Disk Wipe: Disk Structure Wipe");
            techniques.Add("T1499", "Endpoint Denial of Service");
            techniques.Add("T1499/001", "Endpoint Denial of Service: OS Exhaustion Flood");
            techniques.Add("T1499/002", "Endpoint Denial of Service: Service Exhaustion Flood");
            techniques.Add("T1499/003", "Endpoint Denial of Service: Application Exhaustion Flood");
            techniques.Add("T1499/004", "Endpoint Denial of Service: Application or System Exploitation");
            techniques.Add("T1495", "Firmware Corruption");
            techniques.Add("T1490", "Inhibit System Recovery");
            techniques.Add("T1498", "Network Denial of Service");
            techniques.Add("T1498/001", "Network Denial of Service: Direct Network Flood");
            techniques.Add("T1498/002", "Network Denial of Service: Reflection Amplification");
            techniques.Add("T1496", "Resource Hijacking");
            techniques.Add("T1489", "Service Stop");
            techniques.Add("T1529", "System Shutdown/Reboot");

            // Add the TA0040->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0040'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0040'>TA0040 - Impact</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0040' class='collapse' aria-labelledby='heading-TA0040' data-parent='#collapse-enterprise'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion
            
            // tactics/mobile -- count 14 at time of writing
            #region tactics/mobile

            //htmlContent.Add("<optgroup label='Mobile'>");

            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark' id='heading-mobile'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-mobile'>MITRE ATT&CK MOBILE</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-mobile' class='collapse' aria-labelledby='heading-mobile' data-parent='#accordion'>");

            // tactics/mobile/TA0027 -- count 9 at time of writing
            #region tactics TA0027

            techniques = new Dictionary<string, string>();
            techniques.Add("T1475", "Deliver malicious app via authorized app store");
            techniques.Add("T1476", "Deliver malicious app via other means");
            techniques.Add("T1456", "Drive-by compromise");
            techniques.Add("T1458", "Exploit via charging station or PC");
            techniques.Add("T1477", "Exploit via radio interfaces");
            techniques.Add("T1478", "Install insecure of malicious configuration");
            techniques.Add("T1461", "Lockscreen bypass");
            techniques.Add("T1444", "Masquerade as legitimate application");
            techniques.Add("T1474", "Supply chain compromise");

            // Add the TA0027->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0027'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0027'>TA0027 - Initial Access</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0027' class='collapse' aria-labelledby='heading-TA0027' data-parent='#collapse-mobile'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");


            #endregion

            // tactics/mobile/TA0041 -- count 2 at time of writing
            #region tactics TA0041

            techniques = new Dictionary<string, string>();
            techniques.Add("T1402", "Broadcast receivers");
            techniques.Add("T1575", "Native code");
            techniques.Add("T1605", "Command-Line Interface");
            techniques.Add("T1603", "Scheduled Task/Job");

            // Add the TA0041->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0041'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0041'>TA0041 - Execution</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0041' class='collapse' aria-labelledby='heading-TA0041' data-parent='#collapse-mobile'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");


            #endregion

            // tactics/mobile/TA0028 -- count 9 at time of writing
            #region tactics TA0028

            techniques = new Dictionary<string, string>();
            techniques.Add("T1401", "Abuse device administrator access to prevent removal");
            techniques.Add("T1402", "Broadcast receivers");
            techniques.Add("T1540", "Code injection");
            techniques.Add("T1577", "Compromise application executable");
            techniques.Add("T1541", "Foreground persistence");
            techniques.Add("T1403", "Modify cached executable code");
            techniques.Add("T1398", "Modify OS kernal or boot partition");
            techniques.Add("T1400", "Modify system partition");
            techniques.Add("T1399", "Modify trusted execution environment");
            techniques.Add("T1603", "Scheduled Task/Job");

            // Add the TA0028->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0028'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0028'>TA0028 - Persistence</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0028' class='collapse' aria-labelledby='heading-TA0028' data-parent='#collapse-mobile'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");


            #endregion

            // tactics/mobile/TA0029 -- count 3 at time of writing
            #region tactics TA0029

            techniques = new Dictionary<string, string>();
            techniques.Add("T1540", "Code injection");
            techniques.Add("T1401", "Device Administrator Permissions");
            techniques.Add("T1404", "Exploit OS vulnerability");
            techniques.Add("T1405", "Exploit TEE vulnerability");

            // Add the TA0029->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0029'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0029'>TA0029 - Privilege Escalation</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0029' class='collapse' aria-labelledby='heading-TA0029' data-parent='#collapse-mobile'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            // tactics/mobile/TA0030 -- count 18 at time of writing
            #region tactics TA0030

            techniques = new Dictionary<string, string>();
            techniques.Add("T1418", "Application discovery");
            techniques.Add("T1540", "Code injection");
            techniques.Add("T1447", "Delete Device Data");
            techniques.Add("T1446", "Device lockout");
            techniques.Add("T1408", "Disguise root/jailbreak indicators");
            techniques.Add("T1407", "Download new code at runtime");
            techniques.Add("T1523", "Evade analysis environment");
            techniques.Add("T1581", "Geofencing");
            techniques.Add("T1516", "Input injection");
            techniques.Add("T1478", "Install insecure or malicious configuration");
            techniques.Add("T1444", "Masquerade as legitimate application");
            techniques.Add("T1398", "Modify OS kernel or boot partition");
            techniques.Add("T1400", "Modify system partition");
            techniques.Add("T1399", "Modify trusted execution environment");
            techniques.Add("T1575", "Native code");
            techniques.Add("T1406", "Obfuscated files or information");
            techniques.Add("T1508", "Suppress application icon");
            techniques.Add("T1576", "Uninstall malicious application");
            techniques.Add("T1604", "Proxy Through Victim");
            techniques.Add("T1617", "Hooking");
            techniques.Add("T1618", "User Evasion");

            // Add the TA0030->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0030'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0030'>TA0030 - Defense Evasion</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0030' class='collapse' aria-labelledby='heading-TA0030' data-parent='#collapse-mobile'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            // tactics/mobile/TA0031 -- count 11 at time of writing
            #region tactics TA0031

            techniques = new Dictionary<string, string>();
            techniques.Add("T1517", "Access notifications");
            techniques.Add("T1413", "Access sensitive data in device logs");
            techniques.Add("T1409", "Access stored application data");
            techniques.Add("T1414", "Capture clipboard data");
            techniques.Add("T1412", "Capture SMS messages");
            techniques.Add("T1405", "Exploit TEE vulnerability");
            techniques.Add("T1417", "Input capture");
            techniques.Add("T1411", "Input prompt");
            techniques.Add("T1579", "Keychain");
            techniques.Add("T1410", "Network traffic capture or redirection");
            techniques.Add("T1416", "URI Hijacking");
            //techniques.Add("T1415", "URL scheme hijacking");

            // Add the TA0031->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0031'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0031'>TA0031 - Credential Access</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0031' class='collapse' aria-labelledby='heading-TA0031' data-parent='#collapse-mobile'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            // tactics/mobile/TA0032 -- count 9 at time of writing
            #region tactics TA0032

            techniques = new Dictionary<string, string>();
            techniques.Add("T1418", "Application discovery");
            techniques.Add("T1523", "Evade analysis environment");
            techniques.Add("T1420", "File and directory discovery");
            techniques.Add("T1430", "Location tracking");
            techniques.Add("T1423", "Network service scanning");
            techniques.Add("T1424", "Process discovery");
            techniques.Add("T1426", "System infromation discovery");
            techniques.Add("T1422", "System netwrok configuration discovery");
            techniques.Add("T1421", "System network connections discovery");

            // Add the TA0032->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0032'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0032'>TA0032 - Discovery</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0032' class='collapse' aria-labelledby='heading-TA0032' data-parent='#collapse-mobile'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            // tactics/mobile/TA0033 -- count 2 at time of writing
            #region tactics TA0033

            techniques = new Dictionary<string, string>();
            techniques.Add("T1427", "Attack PC via USB connection");
            techniques.Add("T1428", "Exploit enterprise resources");

            // Add the TA0033->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0033'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0033'>TA0033 - Lateral Movement</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0033' class='collapse' aria-labelledby='heading-TA0033' data-parent='#collapse-mobile'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            // tactics/mobile/TA0035 -- count 17 at time of writing
            #region tactics TA0035

            techniques = new Dictionary<string, string>();
            techniques.Add("T1435", "Access calendar entries");
            techniques.Add("T1433", "Access call log");
            techniques.Add("T1432", "Access contact list");
            techniques.Add("T1517", "Access notifications");
            techniques.Add("T1413", "Access sensitive data in device logs");
            techniques.Add("T1409", "Access stored application data");
            techniques.Add("T1429", "Capture audio");
            techniques.Add("T1512", "Capture camera");
            techniques.Add("T1414", "Capture clipboard data");
            techniques.Add("T1412", "Capture SMS messages");
            techniques.Add("T1533", "Data from local system");
            techniques.Add("T1541", "Foreground persistence");
            techniques.Add("T1417", "Input capture");
            techniques.Add("T1430", "Location tracking");
            techniques.Add("T1507", "Network information discovery");
            techniques.Add("T1410", "Network traffic capture or redirection");
            techniques.Add("T1513", "Screen capture");
            techniques.Add("T1616", "Call Control");

            // Add the TA0035->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0035'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0035'>TA0035 - Collection</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0035' class='collapse' aria-labelledby='heading-TA0035' data-parent='#collapse-mobile'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            // tactics/mobile/TA0037 -- count 8 at time of writing
            #region tactics TA0037

            techniques = new Dictionary<string, string>();
            techniques.Add("T1438", "Alternate network mediums");
            techniques.Add("T1436", "Commonly used port");
            techniques.Add("T1520", "Domain generation algorithms");
            techniques.Add("T1544", "Remote file copy");
            techniques.Add("T1437", "Standard application layer protocol");
            techniques.Add("T1521", "Standard cryptographic protocol");
            techniques.Add("T1509", "Uncommonly used port");
            techniques.Add("T1481", "Web service");
            techniques.Add("T1616", "Call Control");

            // Add the TA0037->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0037'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0037'>TA0037 - Command and Control</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0037' class='collapse' aria-labelledby='heading-TA0037' data-parent='#collapse-mobile'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            // tactics/mobile/TA0036 -- count 4 at time of writing
            #region tactics TA0036

            techniques = new Dictionary<string, string>();
            techniques.Add("T1438", "Alternate network mediums");
            techniques.Add("T1436", "Commonly used port");
            techniques.Add("T1532", "Data encrypted");
            techniques.Add("T1437", "Standard application layer protocol");

            // Add the TA0036->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0036'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0036'>TA0036 - Exfiltration</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0036' class='collapse' aria-labelledby='heading-TA0036' data-parent='#collapse-mobile'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            // tactics/mobile/TA0034 -- count 10 at time of writing
            #region tactics TA0034

            techniques = new Dictionary<string, string>();
            techniques.Add("T1448", "Carrier billing fraud");
            techniques.Add("T1510", "Clipboard modification");
            techniques.Add("T1471", "Data encrypted for impact");
            techniques.Add("T1447", "Delete device data");
            techniques.Add("T1446", "Device lockout");
            techniques.Add("T1472", "Generate fraudulent advertising revenue");
            techniques.Add("T1516", "Input injection");
            techniques.Add("T1452", "Manipulate app store rankings or ratings");
            techniques.Add("T1400", "Modify system partition");
            techniques.Add("T1582", "SMS Control");
            techniques.Add("T1616", "Call Control");

            // Add the TA0034->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0034'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0034'>TA0034 - Impact</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0034' class='collapse' aria-labelledby='heading-TA0034' data-parent='#collapse-mobile'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            // tactics/mobile/TA0038 -- count 9 at time of writing
            #region tactics TA0038

            techniques = new Dictionary<string, string>();
            techniques.Add("T1466", "Downgrade to insecure protocols");
            techniques.Add("T1439", "Eavesdrop on insecure network communication");
            techniques.Add("T1449", "Exploit SS7 to Redirect Phone Calls/SMS");
            techniques.Add("T1450", "Exploit SS7 to track device location");
            techniques.Add("T1464", "Jamming or denial of service");
            techniques.Add("T1463", "Manipulate Device Communication");
            techniques.Add("T1467", "Rogue cellular base station");
            techniques.Add("T1465", "Rogue Wi-Fi access points");
            techniques.Add("T1451", "SIM Card Swap");

            // Add the TA0038->techniques to our options list
            htmlContent.Add("<div class='card-body bg-dark' id='TA0038'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0038'>TA0038 - Network Effects</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0038' class='collapse' aria-labelledby='heading-TA0038' data-parent='#collapse-mobile'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            // tactics/mobile/TA0039 -- count 3 at time of writing
            #region tactics TA0039

            techniques = new Dictionary<string, string>();
            techniques.Add("T1470", "Obtain device cloud backups");
            techniques.Add("T1468", "Remotely track device without authorization");
            techniques.Add("T1469", "Remotely wipe data without authorization");

            // Add the TA0039->techniques to our options list
            
            htmlContent.Add("<div class='card-body bg-dark' id='TA0039'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-TA0039'>TA0039 - Remote Service Effects</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-TA0039' class='collapse' aria-labelledby='heading-TA0039' data-parent='#collapse-mobile'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            // tactics/ICS -- count 12 at time of writing -- Attack for ICS last updated 29 April 2021
            #region ICS
            
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark' id='heading-ics'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-ics'>ATT&CK For ICS</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-ics' class='collapse' aria-labelledby='heading-ics' data-parent='#accordion'>");

            #region tactics Collection

            techniques = new Dictionary<string, string>();
            techniques.Add("ics-T0802", "Automated Collection");
            techniques.Add("ics-T0811", "Data from Information Repositories");
            techniques.Add("ics-T0868", "Detect Operating Mode");
            techniques.Add("ics-T0877", "I/O Image");
            techniques.Add("ics-T0830", "Man in the Middle");
            techniques.Add("ics-T0801", "Monitor Process State");
            techniques.Add("ics-T0861", "Point & Tag Identification");
            techniques.Add("ics-T0845", "Program Upload");
            techniques.Add("ics-T0852", "Screen Capture");
            techniques.Add("ics-T0887", "Wireless Sniffing");

            // Add the techniques to our options list

            htmlContent.Add("<div class='card-body bg-dark' id='ics-collection'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-ics-collection'>Collection</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-ics-collection' class='collapse' aria-labelledby='heading-ics-collection' data-parent='#collapse-ics'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            #region tactics Command and Control

            techniques = new Dictionary<string, string>();
            techniques.Add("ics-T0885", "Commonly Used Port");
            techniques.Add("ics-T0884", "Connection Proxy");
            techniques.Add("ics-T0869", "Standard Application Layer Protocol");

            // Add the techniques to our options list

            htmlContent.Add("<div class='card-body bg-dark' id='ics-command-and-control'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-ics-command-and-control'>Command and Control</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-ics-command-and-control' class='collapse' aria-labelledby='heading-ics-command-and-control' data-parent='#collapse-ics'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            #region tactics Discovery

            techniques = new Dictionary<string, string>();
            techniques.Add("ics-T0840", "Network Connection Enumeration");
            techniques.Add("ics-T0842", "Network Sniffing");
            techniques.Add("ics-T0846", "Remote System Discovery");
            techniques.Add("ics-T0888", "Remote System Information Discovery");
            techniques.Add("ics-T0887", "Wireless Sniffing");

            // Add the techniques to our options list

            htmlContent.Add("<div class='card-body bg-dark' id='ics-discovery'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-ics-discovery'>Discovery</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-ics-discovery' class='collapse' aria-labelledby='heading-ics-discovery' data-parent='#collapse-ics'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            #region tactics Evasion

            techniques = new Dictionary<string, string>();
            techniques.Add("ics-T0858", "Change Operating Mode");
            techniques.Add("ics-T0820", "Exploitation for Evasion");
            techniques.Add("ics-T0872", "Indicator Removal on Host");
            techniques.Add("ics-T0849", "Masquerading");
            techniques.Add("ics-T0851", "Rootkit");
            techniques.Add("ics-T0856", "Spoof Reporting Message");

            // Add the techniques to our options list

            htmlContent.Add("<div class='card-body bg-dark' id='ics-evasion'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-ics-evasion'>Evasion</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-ics-evasion' class='collapse' aria-labelledby='heading-ics-evasion' data-parent='#collapse-ics'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            #region tactics Execution

            techniques = new Dictionary<string, string>();
            techniques.Add("ics-T0858", "Change Operating Mode");
            techniques.Add("ics-T0807", "Command-Line Interface");
            techniques.Add("ics-T0871", "Execution through API");
            techniques.Add("ics-T0823", "Graphical User Interface");
            techniques.Add("ics-T0874", "Hooking");
            techniques.Add("ics-T0821", "Modify Controller Tasking");
            techniques.Add("ics-T0834", "Native API");
            techniques.Add("ics-T0853", "Scripting");
            techniques.Add("ics-T0863", "User Execution");

            // Add the techniques to our options list

            htmlContent.Add("<div class='card-body bg-dark' id='ics-execution'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-ics-execution'>Execution</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-ics-execution' class='collapse' aria-labelledby='heading-ics-execution' data-parent='#collapse-ics'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            #region tactics Impact

            techniques = new Dictionary<string, string>();
            techniques.Add("ics-T0879", "Damage to Property");
            techniques.Add("ics-T0813", "Denial of Control");
            techniques.Add("ics-T0815", "Denial of View");
            techniques.Add("ics-T0826", "Loss of Availability");
            techniques.Add("ics-T0827", "Loss of Control");
            techniques.Add("ics-T0828", "Loss of Productivity and Revenue");
            techniques.Add("ics-T0837", "Loss of Protection");
            techniques.Add("ics-T0880", "Loss of Safety");
            techniques.Add("ics-T0829", "Loss of View");
            techniques.Add("ics-T0831", "Manipulation of Control");
            techniques.Add("ics-T0832", "Manipulation of View");
            techniques.Add("ics-T0882", "Theft of Operational Information");

            // Add the techniques to our options list

            htmlContent.Add("<div class='card-body bg-dark' id='ics-impact'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-ics-impact'>Impact</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-ics-impact' class='collapse' aria-labelledby='heading-ics-impact' data-parent='#collapse-ics'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            #region tactics Impair Process Control

            techniques = new Dictionary<string, string>();
            techniques.Add("ics-T0806", "Brute Force I/O");
            techniques.Add("ics-T0836", "Modify Parameter");
            techniques.Add("ics-T0839", "Module Firmware");
            techniques.Add("ics-T0856", "Spoof Reporting Message");
            techniques.Add("ics-T0855", "Unauthorized Command Message");

            // Add the techniques to our options list

            htmlContent.Add("<div class='card-body bg-dark' id='ics-impair-process-control'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-ics-impair-process-control'>Impair Process Control</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-ics-impair-process-control' class='collapse' aria-labelledby='heading-ics-impair-process-control' data-parent='#collapse-ics'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            #region tactics Inhibit Response Function

            techniques = new Dictionary<string, string>();
            techniques.Add("ics-T0800", "Activate Firmware Update Mode");
            techniques.Add("ics-T0878", "Alarm Suppression");
            techniques.Add("ics-T0803", "Block Command Message");
            techniques.Add("ics-T0804", "Block Reporting Message");
            techniques.Add("ics-T0805", "Block Serial COM");
            techniques.Add("ics-T0809", "Data Destruction");
            techniques.Add("ics-T0814", "Denial of Service");
            techniques.Add("ics-T0816", "Device Restart/Shutdown");
            techniques.Add("ics-T0835", "Manipulate I/O Image");
            techniques.Add("ics-T0838", "Modify Alarm Settings");
            techniques.Add("ics-T0851", "Rootkit");
            techniques.Add("ics-T0881", "Service Stop");
            techniques.Add("ics-T0857", "System Firmware");

            // Add the techniques to our options list

            htmlContent.Add("<div class='card-body bg-dark' id='ics-inhibit-response-function'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-ics-inhibit-response-function'>Inhibit Response Function</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-ics-inhibit-response-function' class='collapse' aria-labelledby='heading-ics-inhibit-response-function' data-parent='#collapse-ics'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            #region tactics Initial Access

            techniques = new Dictionary<string, string>();
            techniques.Add("ics-T0810", "Data Historian Compromise");
            techniques.Add("ics-T0817", "Drive-by Compromise");
            techniques.Add("ics-T0818", "Engineering Workstation Compromise");
            techniques.Add("ics-T0819", "Exploit Public-Facing Application");
            techniques.Add("ics-T0866", "Exploitation of Remote Services");
            techniques.Add("ics-T0822", "External Remote Services");
            techniques.Add("ics-T0883", "Internet Accessible Device");
            techniques.Add("ics-T0886", "Remote Services");
            techniques.Add("ics-T0847", "Replication Through Removable Media");
            techniques.Add("ics-T0848", "Rogue Master");
            techniques.Add("ics-T0865", "Spearphishing Attachment");
            techniques.Add("ics-T0862", "Supply Chain Compromise");
            techniques.Add("ics-T0860", "Wireless Compromise");

            // Add the techniques to our options list

            htmlContent.Add("<div class='card-body bg-dark' id='ics-initial-access'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-ics-initial-access'>Initial Access</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-ics-initial-access' class='collapse' aria-labelledby='heading-ics-initial-access' data-parent='#collapse-ics'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            #region tactics Lateral Movement

            techniques = new Dictionary<string, string>();
            techniques.Add("ics-T0812", "Default Credentials");
            techniques.Add("ics-T0866", "Exploitation of Remote Services");
            techniques.Add("ics-T0867", "Lateral Tool Transfer");
            techniques.Add("ics-T0843", "Program Download");
            techniques.Add("ics-T0886", "Remote Services");
            techniques.Add("ics-T0859", "Valid Accounts");

            // Add the techniques to our options list

            htmlContent.Add("<div class='card-body bg-dark' id='ics-lateral-movement'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-ics-lateral-movement'>Lateral Movement</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-ics-lateral-movement' class='collapse' aria-labelledby='heading-ics-lateral-movement' data-parent='#collapse-ics'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            #region tactics Persistence

            techniques = new Dictionary<string, string>();
            techniques.Add("ics-T0889", "Modify Program");
            techniques.Add("ics-T0839", "Module Firmware");
            techniques.Add("ics-T0873", "Project File Infection");
            techniques.Add("ics-T0857", "System Firmware");
            techniques.Add("ics-T0859", "Valid Accounts");

            // Add the techniques to our options list

            htmlContent.Add("<div class='card-body bg-dark' id='ics-persistence'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-ics-persistence'>Persistence</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-ics-persistence' class='collapse' aria-labelledby='heading-ics-persistence' data-parent='#collapse-ics'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            #region tactics Privilege Escalation

            techniques = new Dictionary<string, string>();
            techniques.Add("ics-T0890", "Exploitation for Privilege Escalation");
            techniques.Add("ics-T0874", "Hooking");

            // Add the techniques to our options list

            htmlContent.Add("<div class='card-body bg-dark' id='ics-privilege-escalation'>");
            htmlContent.Add("<div class='card'>");
            htmlContent.Add("<div class='card-header bg-dark'>");
            htmlContent.Add("<a href='#' data-toggle='collapse' data-target='#collapse-ics-privilege-escalation'>Privilege Escalation</a>");
            htmlContent.Add("</div>");
            htmlContent.Add("<div id='collapse-ics-privilege-escalation' class='collapse' aria-labelledby='heading-ics-privilege-escalation' data-parent='#collapse-ics'>");

            htmlContent = AddHtml(htmlContent, techniques, selected);
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            htmlContent.Add("</div>");
            htmlContent.Add("</div>");

            #endregion

            htmlContent.Add("</div>");
        }

        private List<string> AddHtml(List<string> htmlContent, Dictionary<string, string> techniques, string selected)
        {
            foreach (var technique in techniques)
            {
                htmlContent.Add("<div class='card-body bg-dark' id='" + technique.Key + "'>");
                htmlContent.Add("<div class='card' style='border:none;'>");
                htmlContent.Add("<div class='card-header bg-dark' id='heading-" + technique.Key + "'>");
                
                if (selected == technique.Key)
                {
                    htmlContent.Add("<input class='form-check-input' value='" + technique.Key + "' id='radio-" + technique.Key + "' type='radio' name='mitreId' checked='checked' onclick='document.getElementById(\"selectedTechniqueSpan\").innerHTML=\"" + technique.Key + " - " + technique.Value + "\";'>");
                }
                else { htmlContent.Add("<input class='form-check-input' value='" + technique.Key + "' id='radio-" + technique.Key + "' type='radio' name='mitreId' onclick='document.getElementById(\"selectedTechniqueSpan\").innerHTML=\"" + technique.Key + " - " + technique.Value + "\";'>"); }
                
                htmlContent.Add("<label class='form-check-label' for='radio-" + technique.Key + "'>" + technique.Key.ToUpper() + " - " + technique.Value + "</label>");
                string url = @"https://attack.mitre.org/techniques/";
                string tech = technique.Key + "/";
                if (technique.Key.StartsWith("ics-")) { url = @"https://collaborate.mitre.org/attackics/index.php/Technique/"; tech = technique.Key.Substring(4); }
                htmlContent.Add("<a style='margin-left: 5px;' href='" + url + tech +"' target='_blank'>");
                htmlContent.Add("<svg width='1em' height='1em' viewBox='0 0 16 16' class='bi bi-info-square' fill='currentColor' xmlns='http://www.w3.org/2000/svg'>");
                htmlContent.Add("<path fill-rule='evenodd' d='M14 1H2a1 1 0 0 0-1 1v12a1 1 0 0 0 1 1h12a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1zM2 0a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2H2z' />");
                htmlContent.Add("<path d='M8.93 6.588l-2.29.287-.082.38.45.083c.294.07.352.176.288.469l-.738 3.468c-.194.897.105 1.319.808 1.319.545 0 1.178-.252 1.465-.598l.088-.416c-.2.176-.492.246-.686.246-.275 0-.375-.193-.304-.533L8.93 6.588z' />");
                htmlContent.Add("<circle cx='8' cy='4.5' r='1' />");
                htmlContent.Add("</svg>");
                htmlContent.Add("</a>");

                htmlContent.Add("</div>");
                htmlContent.Add("</div>");
                htmlContent.Add("</div>");
            }

            return htmlContent;
        }
    }
}
