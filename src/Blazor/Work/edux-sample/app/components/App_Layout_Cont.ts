"use client";

import { useState, useEffect, ReactNode } from "react";

interface DashboardProps {
  renderAppLayout: (props: {
    toggleSidebar: () => void;
    toggleTheme: () => void;
    isDark: boolean;
    sidebarOpen: boolean;
    closeSidebar: () => void;
  }) => ReactNode;
}

export default function Dashboard({ renderAppLayout }: DashboardProps) {
  const [sidebarOpen, setSidebarOpen] = useState(false);
  const [isDark, setIsDark] = useState(false);

  useEffect(() => {
    const savedTheme = localStorage.getItem("theme");
    const systemDark = window.matchMedia("(prefers-color-scheme: dark)").matches;

    if (savedTheme === "dark" || (!savedTheme && systemDark)) {
      setIsDark(true);
      document.documentElement.classList.add("dark");
    } else {
      setIsDark(false);
      document.documentElement.classList.remove("dark");
    }
  }, []);

  const toggleSidebar = () => setSidebarOpen(!sidebarOpen);
  const closeSidebar = () => setSidebarOpen(false);

  const toggleTheme = () => {
    const newTheme = !isDark;
    setIsDark(newTheme);

    if (newTheme) {
      document.documentElement.classList.add("dark");
      localStorage.setItem("theme", "dark");
    } else {
      document.documentElement.classList.remove("dark");
      localStorage.setItem("theme", "light");
    }
  };

  return renderAppLayout({
    toggleSidebar,
    toggleTheme,
    isDark,
    sidebarOpen,
    closeSidebar,
  });
}
