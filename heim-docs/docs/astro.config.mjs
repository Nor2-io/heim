// @ts-check
import { defineConfig } from "astro/config";
import starlight from "@astrojs/starlight";

// https://astro.build/config
export default defineConfig({
  base: "/heim/docs",
  site: "https://cloud.heim.dev",
  integrations: [
    starlight({
      title: "Heim Docs",
      logo: {
        light: "./src/assets/logo-light.svg",
        dark: "./src/assets/logo-dark.svg",
        replacesTitle: true,
      },
      favicon: "./src/assets/favicon.png",
      social: [
        {
          icon: "github",
          label: "GitHub",
          href: "https://github.com/Nor2-io/heim",
        },
        {
          icon: "discord",
          label: "Discord",
          href: "https://discord.com/invite/heim",
        },
      ],
      sidebar: [
        {
          label: "Start Here",
          autogenerate: { directory: "start-here" },
        },
        {
          label: "Application",
          autogenerate: { directory: "application" },
        },
        {
          label: "Organization",
          autogenerate: { directory: "organization" },
        },
        {
          label: "CLI",
          autogenerate: { directory: "cli" },
        },
        {
          label: "Templates",
          autogenerate: { directory: "templates" },
        },
        {
          label: "Guides",
          autogenerate: { directory: "guides" },
        },
      ],
      customCss: ["./src/styles/custom.css"],
    }),
  ],
});
