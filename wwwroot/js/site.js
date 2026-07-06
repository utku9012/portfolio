const translations = {
  en: {
    "nav.about": "About",
    "nav.experiences": "Experiences",
    "nav.projects": "Projects",
    "nav.education": "Education",
    "nav.skills": "Skills",
    "nav.certifications": "Certifications",
    "section.experiences": "Experiences",
    "section.projects": "Projects",
    "section.education": "Education",
    "section.skills": "Skills",
    "section.certifications": "Certifications",
    "link.viewProject": "View project",
    "link.viewCredential": "View credential",
    "link.downloadCv": "Download CV",
    "admin.cms": "Portfolio CMS",
    "admin.panel": "Admin Panel",
    "admin.logout": "Logout",
    "admin.about": "About",
    "admin.saveAbout": "Save about",
    "admin.addExperience": "Add experience",
    "admin.addProject": "Add project",
    "admin.addEducation": "Add education",
    "admin.addSkill": "Add skill",
    "admin.addCertification": "Add certification",
    "admin.dragReorder": "Drag to reorder",
    "field.firstName": "First name",
    "field.lastName": "Last name",
    "field.title": "Title",
    "field.titleTr": "Title TR",
    "field.email": "Email",
    "field.phone": "Phone",
    "field.location": "Location",
    "field.summary": "Summary",
    "field.summaryTr": "Summary TR",
    "field.linkedin": "LinkedIn",
    "field.github": "GitHub",
    "field.photoUrl": "Photo URL",
    "field.profileImageUpload": "Profile image upload",
    "field.cvUrl": "CV URL",
    "field.cvUpload": "CV upload",
    "field.projectImageUrl": "Project image URL",
    "field.projectImageUpload": "Project image upload",
    "field.grade": "GPA",
    "button.save": "Save",
    "button.delete": "Delete",
    "login.description": "Sign in to manage your portfolio content.",
    "login.password": "Password",
    "login.signIn": "Sign in",
    "login.back": "Back to portfolio",
    "status.savingOrder": "Saving project order...",
    "status.orderFailed": "Project order could not be saved.",
    "status.orderSaved": "Project order saved."
  },
  tr: {
    "nav.about": "Hakk\u0131mda",
    "nav.experiences": "Deneyimler",
    "nav.projects": "Projeler",
    "nav.education": "E\u011fitim",
    "nav.skills": "Yetenekler",
    "nav.certifications": "Sertifikalar",
    "section.experiences": "Deneyimler",
    "section.projects": "Projeler",
    "section.education": "E\u011fitim",
    "section.skills": "Yetenekler",
    "section.certifications": "Sertifikalar",
    "link.viewProject": "Projeyi g\u00f6r",
    "link.viewCredential": "Sertifikay\u0131 g\u00f6r",
    "link.downloadCv": "CV indir",
    "admin.cms": "Portfolyo CMS",
    "admin.panel": "Admin Panel",
    "admin.logout": "\u00c7\u0131k\u0131\u015f yap",
    "admin.about": "Hakk\u0131mda",
    "admin.saveAbout": "Hakk\u0131mda bilgisini kaydet",
    "admin.addExperience": "Deneyim ekle",
    "admin.addProject": "Proje ekle",
    "admin.addEducation": "E\u011fitim ekle",
    "admin.addSkill": "Yetenek ekle",
    "admin.addCertification": "Sertifika ekle",
    "admin.dragReorder": "S\u0131ralamak i\u00e7in s\u00fcr\u00fckle",
    "field.firstName": "Ad",
    "field.lastName": "Soyad",
    "field.title": "Unvan",
    "field.titleTr": "Unvan TR",
    "field.email": "E-posta",
    "field.phone": "Telefon",
    "field.location": "Konum",
    "field.summary": "\u00d6zet",
    "field.summaryTr": "\u00d6zet TR",
    "field.linkedin": "LinkedIn",
    "field.github": "GitHub",
    "field.photoUrl": "Foto\u011fraf URL",
    "field.profileImageUpload": "Profil foto\u011fraf\u0131 y\u00fckle",
    "field.cvUrl": "CV URL",
    "field.cvUpload": "CV y\u00fckle",
    "field.projectImageUrl": "Proje g\u00f6rsel URL",
    "field.projectImageUpload": "Proje g\u00f6rseli y\u00fckle",
    "field.grade": "GNO",
    "button.save": "Kaydet",
    "button.delete": "Sil",
    "login.description": "Portfolyo i\u00e7eri\u011fini y\u00f6netmek i\u00e7in giri\u015f yap.",
    "login.password": "\u015eifre",
    "login.signIn": "Giri\u015f yap",
    "login.back": "Portfolyoya d\u00f6n",
    "status.savingOrder": "Proje s\u0131ras\u0131 kaydediliyor...",
    "status.orderFailed": "Proje s\u0131ras\u0131 kaydedilemedi.",
    "status.orderSaved": "Proje s\u0131ras\u0131 kaydedildi."
  }
};

const getLanguage = () => localStorage.getItem("portfolioLanguage") || "tr";
const getTheme = () => localStorage.getItem("portfolioTheme") || "dark";
const translate = (key) => translations[getLanguage()]?.[key] || translations.en[key] || key;

const applyLanguage = () => {
  const language = getLanguage();
  document.documentElement.lang = language;
  document.body.dataset.language = language;

  document.querySelectorAll("[data-i18n]").forEach((element) => {
    element.textContent = translate(element.dataset.i18n);
  });

  document.querySelectorAll("[data-i18n-title]").forEach((element) => {
    element.title = translate(element.dataset.i18nTitle);
  });

  document.querySelectorAll("[data-i18n-aria-label]").forEach((element) => {
    element.setAttribute("aria-label", translate(element.dataset.i18nAriaLabel));
  });

  const languageToggle = document.querySelector("[data-language-toggle]");

  if (languageToggle) {
    languageToggle.textContent = language === "tr" ? "EN" : "TR";
    languageToggle.title = language === "tr" ? "English" : "T\u00fcrk\u00e7e";
    languageToggle.setAttribute("aria-label", language === "tr" ? "Switch to English" : "T\u00fcrk\u00e7eye ge\u00e7");
  }
};

const applyTheme = () => {
  const theme = getTheme();
  document.body.dataset.theme = theme;

  const themeToggleIcon = document.querySelector("[data-theme-toggle] i");
  const themeToggle = document.querySelector("[data-theme-toggle]");

  if (themeToggleIcon) {
    themeToggleIcon.className = theme === "dark" ? "fas fa-sun" : "fas fa-moon";
  }

  if (themeToggle) {
    themeToggle.title = theme === "dark" ? "Light theme" : "Dark theme";
    themeToggle.setAttribute("aria-label", theme === "dark" ? "Switch to light theme" : "Switch to dark theme");
  }
};

document.querySelector("[data-language-toggle]")?.addEventListener("click", () => {
  localStorage.setItem("portfolioLanguage", getLanguage() === "tr" ? "en" : "tr");
  applyLanguage();
});

document.querySelector("[data-theme-toggle]")?.addEventListener("click", () => {
  localStorage.setItem("portfolioTheme", getTheme() === "dark" ? "light" : "dark");
  applyTheme();
});

applyLanguage();
applyTheme();

const projectDragList = document.querySelector("[data-project-reorder-url]");

if (projectDragList) {
  const status = document.querySelector("[data-project-drag-status]");
  let draggedItem = null;

  const setStatus = (message) => {
    if (status) {
      status.textContent = message;
    }
  };

  const getDragAfterElement = (container, y) => {
    const draggableItems = [...container.querySelectorAll(".project-sort-item:not(.dragging)")];

    return draggableItems.reduce(
      (closest, child) => {
        const box = child.getBoundingClientRect();
        const offset = y - box.top - box.height / 2;

        if (offset < 0 && offset > closest.offset) {
          return { offset, element: child };
        }

        return closest;
      },
      { offset: Number.NEGATIVE_INFINITY, element: null }
    ).element;
  };

  const saveProjectOrder = async () => {
    const token = projectDragList.querySelector("input[name='__RequestVerificationToken']")?.value;
    const formData = new FormData();

    projectDragList.querySelectorAll(".project-sort-item").forEach((item) => {
      formData.append("projectIds", item.dataset.projectId);
    });

    if (token) {
      formData.append("__RequestVerificationToken", token);
    }

    setStatus(translate("status.savingOrder"));

    const response = await fetch(projectDragList.dataset.projectReorderUrl, {
      method: "POST",
      body: formData
    });

    if (!response.ok) {
      setStatus(translate("status.orderFailed"));
      return;
    }

    projectDragList.querySelectorAll(".project-sort-item").forEach((item, index) => {
      const displayOrderInput = item.querySelector("input[name='DisplayOrder']");

      if (displayOrderInput) {
        displayOrderInput.value = index + 1;
      }
    });

    setStatus(translate("status.orderSaved"));
  };

  projectDragList.addEventListener("dragstart", (event) => {
    const item = event.target.closest(".project-sort-item");

    if (!item) {
      return;
    }

    draggedItem = item;
    item.classList.add("dragging");
    event.dataTransfer.effectAllowed = "move";
  });

  projectDragList.addEventListener("dragend", async () => {
    if (!draggedItem) {
      return;
    }

    draggedItem.classList.remove("dragging");
    draggedItem = null;

    try {
      await saveProjectOrder();
    } catch {
      setStatus(translate("status.orderFailed"));
    }
  });

  projectDragList.addEventListener("dragover", (event) => {
    event.preventDefault();

    const afterElement = getDragAfterElement(projectDragList, event.clientY);

    if (!draggedItem) {
      return;
    }

    if (afterElement === null) {
      projectDragList.appendChild(draggedItem);
    } else {
      projectDragList.insertBefore(draggedItem, afterElement);
    }
  });
}
