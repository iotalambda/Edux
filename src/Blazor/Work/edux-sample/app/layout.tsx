"use client";

import { Geist, Geist_Mono } from "next/font/google";
import "./globals.css";
import App_Navbar_Pres from "./components/App_Navbar_Pres";
import App_Sidebar_Pres from "./components/App_Sidebar_Pres";
import HmrHeartbeat from "./components/App_HmrHeartbeat_Cont";
import App_Layout_Cont from "./components/App_Layout_Cont";

const geistSans = Geist({
  variable: "--font-geist-sans",
  subsets: ["latin"],
});

const geistMono = Geist_Mono({
  variable: "--font-geist-mono",
  subsets: ["latin"],
});

// export const metadata: Metadata = {
//   title: "Dashboard App",
//   description: "A responsive dashboard with navigation",
// };

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body className={`${geistSans.variable} ${geistMono.variable} antialiased dark:bg-gray-950`}>
        <HmrHeartbeat />
        <App_Layout_Cont
          renderAppLayout={(props) => (
            <div className="min-h-screen flex flex-col">
              <App_Navbar_Pres
                onToggleSidebar={props.toggleSidebar}
                onToggleTheme={props.toggleTheme}
                isDark={props.isDark}
              />
              <div className="flex flex-1">
                <App_Sidebar_Pres isOpen={props.sidebarOpen} onClose={props.closeSidebar} />
                <main>{children}</main>
              </div>
            </div>
          )}
        />
      </body>
    </html>
  );
}
