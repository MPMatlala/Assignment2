# CyberBot — Cybersecurity Awareness Chatbot

A WPF desktop application built in C# (.NET) that educates users on cybersecurity topics through an interactive chat interface.

---

## Overview

CyberBot is an AI-style chatbot that answers questions related to cybersecurity concepts such as phishing, passwords, firewalls, VPNs, fraud, and more. It also detects basic user sentiment (frustrated, confused, happy, sad, etc.) and responds empathetically. The app greets users with a voice message and provides a clean, multi-page chat experience.

---

## Features

- **Voice Greeting** — Plays an audio greeting (`greeting.wav`) when the application launches.
- **Username Recognition** — Saves user names to a local text file (`user_names.txt`) and welcomes returning users.
- **Keyword Matching** — Scores each user message against a library of cybersecurity answers using keyword frequency to select the best response.
- **Stopword Filtering** — Ignores common filler words (a, the, and, is, etc.) to focus on meaningful keywords.
- **Sentiment Detection** — Recognises emotional keywords (frustrated, confused, worried, happy, sad, angry) and replies with appropriate empathy.
- **Periodic Safety Reminder** — Every 3 questions, appends a general safety tip to the chatbot's response.
- **Exit Commands** — Typing `exit`, `quit`, or `bye` triggers a farewell message and closes the application.
- **Multi-Page UI** — Three distinct pages: Home, Username Entry, and Chat.

---

## Project Structure

| File / Class | Description |
|---|---|
| `MainWindow.xaml` | UI layout — home page, username page, and chat page |
| `MainWindow.xaml.cs` | Main logic — button handlers, AI response engine, message display |
| `respond.cs` | Populates the answer list and stopword list |
| `user_name.cs` | Handles name submission, file storage, and returning-user detection |
| `voice_greeting.cs` | Plays the greeting WAV file on startup |
| `greeting.wav` | Audio file played on launch (must be in project root) |
| `myLogo.jpg` | Logo image shown on the home page |
| `user_names.txt` | Auto-created file that stores previously seen usernames |

---

## How It Works

### Response Engine (`ai_check`)

1. The user's input is sanitised (special characters removed).
2. The cleaned text is split into individual words.
3. Stopwords and short words (< 3 characters) are filtered out.
4. Each remaining word is matched against the stored answer strings.
5. Each matching answer receives a score (+1 per keyword hit).
6. The answer with the highest score is selected and displayed.
7. If no answer matches, a random fallback message is shown.

### Answer Format (`respond.cs`)

Each answer string starts with its **topic keyword** followed by the actual response text:

```
"phishing phishing is a scam where attackers pretend to be trusted sources..."
"password a password is used to secure access to your accounts or devices."
"frustrated i understand you're frustrated. let's work through this together."
```

This means the topic word itself also contributes to matching — a user asking about "phishing" will score against all phishing answers.

---

## Topics Covered

| Category | Keywords |
|---|---|
| Greetings | greeting, hello, hi |
| Purpose | purpose, what, help |
| Cybersecurity | cybersecurity, security, protect |
| Phishing | phishing, scam, fake |
| Firewall | firewall, network, block |
| Passwords | password, strong, secure |
| Hacked Accounts | hacked, account, compromised |
| Fraud | fraud, bank, financial |
| Malicious Bots | malicious, chatbot, fake |
| VPN | vpn, privacy, encrypt |
| Sentiment | frustrated, confused, worried, happy, sad, angry |

---

## Requirements

- **IDE:** Visual Studio 2019 or later
- **Framework:** .NET Framework 4.7.2+ (WPF)
- **Language:** C# 7+
- **OS:** Windows (WPF is Windows-only)

---

## Setup & Running

1. Clone or download the repository.
2. Open `Assignment2.sln` in Visual Studio.
3. Ensure `greeting.wav` is in the **project root** (same folder as the `.csproj` file).
4. Ensure `myLogo.jpg` is added to the project with **Build Action: Resource**.
5. Press **F5** or click **Start** to run the application.

> **Note:** The voice greeting uses a path replacement from `\bin\Debug\` back to the project root. If you run from a different output folder (e.g. `Release`), update the path in `voice_greeting.cs`.

---

## Known Limitations

- The keyword matcher uses simple substring matching — it does not understand context or sentence meaning.
- Only one answer is returned per message (the highest-scoring match).
- `user_names.txt` is stored in the application's working directory; no encryption or privacy protection is applied.
- The `user_name` class references the `demAssignment2` namespace, which differs from the main `Assignment2` namespace — ensure namespaces are consistent to avoid build errors.

---

## Author

Assignment 2 — Cybersecurity Awareness Chatbot  
Built with C# and WPF
