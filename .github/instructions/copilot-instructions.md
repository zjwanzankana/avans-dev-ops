# GitHub Copilot Quality and Behavior Guidelines

## Section 1: Core Behavior and Anti-Token Saving

### 1.1 Completeness and Accuracy (ANTI-DUMMY CODE)

- **ALL code generation MUST include the full, working implementation.**
- **AVOID AND STAY AWAY** from placeholders, dummy values, `// TODO` comments, `FIXME`, or skeleton code. If a function or class is requested, the implementation must be immediately usable and complete.
- Responses must provide a direct solution. Never deliver partial code intended to be improved later — the output must represent the final requested state.

### 1.2 Context and API Accuracy

- **NEVER IGNORE** the context of open files, given instructions, or explicit API documentation.
- Follow API object names, function names, and syntax **EXACTLY** as specified in the prompt or surrounding code.
- Validate API references, data types, and object structures thoroughly before writing any code.

### 1.3 Structure and Readability (ANTI-NESTING)

- Keep generated code **flat, readable, and optimal**.
- **AVOID UNNECESSARY NESTING**, such as excessive indentation, deeply nested `if/else` blocks, complex ternary operators, or single-line expressions that reduce clarity.
- Prefer **early returns** and avoid overly complex logic.

---

## Section 2: Agent Mode Optimization

### 2.1 Agent Behavior and Reasoning

- **Agent Mode: ALWAYS take the necessary time** for complex tasks (those requiring more than two steps).
- **FORCE the reasoning step** — build a detailed step-by-step plan and validate it before writing any code. The reasoning phase is critical and must not be skipped or shortened.
- Perform a **proactive review** of generated code for common issues (null checks, correct async handling, edge cases, etc.) **before** presenting the final code.

### 2.2 Transparency

- After completing any operation in Agent Mode, provide a **concise summary** of the changes made and **explain key design choices**.

---

## Section 3: Minimalism, Efficiency, and Dependency Management

### 3.1 Dependency Minimalism (ANTI-BLOAT)

- **NEVER use external libraries or dependencies** in generated code unless they are absolutely essential or explicitly requested in the prompt.
- This is a **FAILURE CONDITION**: code bloat, unnecessary imports, and excessive dependency trees are unacceptable.
- Always **prefer built-in functions** from the standard language/runtime (Native JavaScript, Python Standard Library, Java Core Libraries, etc.) over external packages.

### 3.2 Pure Code Implementation

- For functionality such as string manipulation, array operations, date handling, or basic HTTP requests: **implement the logic purely in the language** instead of importing helper libraries.
- The generated code must be **efficient** and **lightweight**, aiming for top performance on web standards (like Lighthouse or Observatory).
- This **“Minimal Dependencies, Maximum Native Code”** principle applies to **ALL** generated code, regardless of the language or platform.

### 3.3 Justification and Necessity

- If an external dependency is **unavoidable** (e.g., a framework or major API wrapper), you **MUST** provide a **detailed justification** during the reasoning phase explaining why built-in solutions are insufficient. The justification must come **before** code generation.

---

## Section 4: Code Quality and Idiomatic Standards

### 4.1 Idiom and Modernity

- **Generate code** using the most modern and idiomatic patterns for the chosen language and framework.
- Strictly adhere to **best practices** for the detected framework (e.g., Hooks in React, Decorators/Modules in NestJS/Angular, modern ES6+ syntax in JavaScript/TypeScript).

### 4.2 Modularity and Separation of Concerns (SoC)

- Always aim for the highest level of **modularity and separation of concerns**.
- Break complex logic into **small, testable, reusable units** (functions, classes, modules), unless the prompt explicitly requests a single function.
- Avoid long methods or classes with **multiple responsibilities**.

---

## Section 5: Reasoning and Documentation

### 5.1 Deep Reasoning

- **Analyze the request** thoroughly. Consider at least one **alternative implementation** and briefly explain why the chosen one is superior.

### 5.2 Documentation and Explanation

- Add **clear, essential JSDoc/TSDoc/XML documentation** to every new or modified public function, class, or method.
- The documentation must include **`params`**, **`returns`**, and a **concise, clear description** of the functionality.

### 5.3 Strict Conformance

- The rules in this document (Sections 1 through 5) are **NON-NEGOTIABLE** and take **highest priority** in every generation.
- **Violation** of these rules constitutes a **failure** to execute the task properly.
