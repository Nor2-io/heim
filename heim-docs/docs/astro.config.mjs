// @ts-check
import { defineConfig } from 'astro/config';
import starlight from '@astrojs/starlight';

// https://astro.build/config
export default defineConfig({
	base: '/heim/docs',
	integrations: [
		starlight({
			title: 'Heim Docs',
			social: [
				{ icon: 'github', label: 'GitHub', href: 'https://github.com/Nor2-io/heim'},
				{ icon: 'discord', label: 'Discord', href: 'https://discord.com/invite/heim'}
			],
			sidebar: [
				{
					label: 'Start Here',
					autogenerate: { directory: 'start-here' },
				},
				{
					label: 'CLI',
					autogenerate: { directory: 'cli' },
				},
				// {
				// 	label: 'Runtime',
				// 	autogenerate: { directory: 'runtime' },
				// },
				{
					label: 'Templates',
					autogenerate: { directory: 'templates' }
				},
				{
					label: 'Guides',
					//items: [
					//	// Each item here is one entry in the navigation menu.
					//	{ label: 'Example Guide', slug: 'guides/example' },
					//],
					autogenerate: { directory: 'guides' },
				},
				//{
				//	label: 'Integrations',
				//	autogenerate: { directory: 'integrations' },
				//},
			],
			customCss: [
				'./src/styles/custom.css'
			]
		}),
	],
});
