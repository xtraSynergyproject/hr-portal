const navigation = () => {
  return [
    {
      icon: 'mdi:home-outline',
      title: 'Dashboards',
      children: [
        {
          title: 'CRM',
          icon: 'mdi:chart-donut',
          path: '/dashboards/crm'
        },
        {
          icon: 'mdi:chart-timeline-variant',
          title: 'Analytics',
          path: '/dashboards/analytics'
        },
        {
          icon: 'mdi:cart-outline',
          title: 'eCommerce',
          path: '/dashboards/ecommerce'
        }
      ]
    },
    {
      icon: 'mdi:apps',
      title: 'Apps',
      children: [
        {
          title: 'Email',
          icon: 'mdi:email-outline',
          path: '/apps/email'
        },
        {
          title: 'Chat',
          icon: 'mdi:message-outline',
          path: '/apps/chat'
        },
        {
          title: 'Calendar',
          icon: 'mdi:calendar-blank-outline',
          path: '/apps/calendar'
        },
        {
          title: 'Invoice',
          icon: 'mdi:file-document-outline',
          children: [
            {
              title: 'List',
              path: '/apps/invoice/list'
            },
            {
              title: 'Preview',
              path: '/apps/invoice/preview'
            },
            {
              title: 'Edit',
              path: '/apps/invoice/edit'
            },
            {
              title: 'Add',
              path: '/apps/invoice/add'
            }
          ]
        },
        {
          title: 'User',
          icon: 'mdi:account-outline',
          children: [
            {
              title: 'List',
              path: '/apps/user/list'
            },
            {
              title: 'View',
              children: [
                {
                  title: 'Overview',
                  path: '/apps/user/view/overview'
                },
                {
                  title: 'Security',
                  path: '/apps/user/view/security'
                },
                {
                  title: 'Billing & Plans',
                  path: '/apps/user/view/billing-plan'
                },
                {
                  title: 'Notifications',
                  path: '/apps/user/view/notification'
                },
                {
                  title: 'Connection',
                  path: '/apps/user/view/connection'
                }
              ]
            }
          ]
        },
        {
          title: 'Roles & Permissions',
          icon: 'mdi:shield-outline',
          children: [
            {
              title: 'Roles',
              path: '/apps/roles'
            },
            {
              title: 'Permissions',
              path: '/apps/permissions'
            }
          ]
        }
      ]
    },
    {
      icon: 'mdi:palette-swatch-outline',
      title: 'UI',
      children: [
        {
          title: 'Typography',
          icon: 'mdi:format-letter-case',
          path: '/ui/typography'
        },
        {
          title: 'Icons',
          path: '/ui/icons',
          icon: 'mdi:google-circles-extended'
        },
        {
          title: 'Cards',
          icon: 'mdi:credit-card-outline',
          children: [
            {
              title: 'Basic',
              path: '/ui/cards/basic'
            },
            {
              title: 'Advanced',
              path: '/ui/cards/advanced'
            },
            {
              title: 'Statistics',
              path: '/ui/cards/statistics'
            },
            {
              title: 'Widgets',
              path: '/ui/cards/widgets'
            },
            {
              title: 'Gamification',
              path: '/ui/cards/gamification'
            },
            {
              title: 'Actions',
              path: '/ui/cards/actions'
            }
          ]
        },
        {
          title: 'Components',
          icon: 'mdi:archive-outline',
          children: [
            {
              title: 'Accordion',
              path: '/components/accordion'
            },
            {
              title: 'Alerts',
              path: '/components/alerts'
            },
            {
              title: 'Avatars',
              path: '/components/avatars'
            },
            {
              title: 'Badges',
              path: '/components/badges'
            },
            {
              title: 'Buttons',
              path: '/components/buttons'
            },
            {
              title: 'Button Group',
              path: '/components/button-group'
            },
            {
              title: 'Chips',
              path: '/components/chips'
            },
            {
              title: 'Dialogs',
              path: '/components/dialogs'
            },
            {
              title: 'List',
              path: '/components/list'
            },
            {
              title: 'Menu',
              path: '/components/menu'
            },
            {
              title: 'Pagination',
              path: '/components/pagination'
            },
            {
              title: 'Ratings',
              path: '/components/ratings'
            },
            {
              title: 'Snackbar',
              path: '/components/snackbar'
            },
            {
              title: 'Swiper',
              path: '/components/swiper'
            },
            {
              title: 'Tabs',
              path: '/components/tabs'
            },
            {
              title: 'Timeline',
              path: '/components/timeline'
            },
            {
              title: 'Toasts',
              path: '/components/toast'
            },
            {
              title: 'Tree View',
              path: '/components/tree-view'
            },
            {
              title: 'More',
              path: '/components/more'
            },
          ]
        }
      ]
    },
    {
      icon: 'mdi:file-document-outline',
      title: 'Pages',
      children: [
        {
          title: 'User Profile',
          icon: 'mdi:card-account-details-outline',
          children: [
            {
              title: 'Profile',
              path: '/pages/user-profile/profile'
            },
            {
              title: 'Teams',
              path: '/pages/user-profile/teams'
            },
            {
              title: 'Projects',
              path: '/pages/user-profile/projects'
            },
            {
              title: 'Connections',
              path: '/pages/user-profile/connections'
            }
          ]
        },
        {
          icon: 'mdi:account-cog-outline',
          title: 'Account Settings',
          children: [
            {
              title: 'Account',
              path: '/pages/account-settings/account'
            },
            {
              title: 'Security',
              path: '/pages/account-settings/security'
            },
            {
              title: 'Billing',
              path: '/pages/account-settings/billing'
            },
            {
              title: 'Notifications',
              path: '/pages/account-settings/notifications'
            },
            {
              title: 'Connections',
              path: '/pages/account-settings/connections'
            }
          ]
        },
        {
          title: 'FAQ',
          path: '/pages/faq',
          icon: 'mdi:help-circle-outline'
        },
        {
          title: 'Help Center',
          icon: 'mdi:help-circle-outline',
          path: '/pages/help-center'
        },
        {
          title: 'Pricing',
          icon: 'mdi:currency-usd',
          path: '/pages/pricing'
        },
      
      
      ]
    },
    {
      title: 'Charts',
      icon: 'mdi:chart-donut',
      children: [
        {
          title: 'Apex',
          icon: 'mdi:chart-line',
          path: '/charts/apex-charts'
        },
        {
          title: 'Recharts',
          icon: 'mdi:chart-bell-curve-cumulative',
          path: '/charts/recharts'
        },
        {
          title: 'ChartJS',
          path: '/charts/chartjs',
          icon: 'mdi:chart-bell-curve'
        }
      ]
    },
    {
      title: 'Others',
      icon: 'mdi:dots-horizontal',
      children: [
        {
          path: '/acl',
          action: 'read',
          subject: 'acl-page',
          icon: 'mdi:shield-outline',
          title: 'Access Control'
        },
        {
          title: 'Menu Levels',
          icon: 'mdi:menu',
          children: [
            {
              title: 'Menu Level 2.1'
            },
            {
              title: 'Menu Level 2.2',
              children: [
                {
                  title: 'Menu Level 3.1'
                },
                {
                  title: 'Menu Level 3.2'
                }
              ]
            }
          ]
        },
        {
          title: 'Disabled Menu',
          icon: 'mdi:eye-off-outline',
          disabled: true
        },
        {
          title: 'Raise Support',
          icon: 'mdi:lifebuoy',
          externalLink: true,
          openInNewTab: true,
          path: 'https://themeselection.com/support'
        },
        {
          title: 'Documentation',
          icon: 'mdi:file-document-outline',
          externalLink: true,
          openInNewTab: true,
          path: 'https://demos.themeselection.com/marketplace/materio-mui-react-nextjs-admin-template/documentation'
        }
      ]
    }
  ]
}

export default navigation
