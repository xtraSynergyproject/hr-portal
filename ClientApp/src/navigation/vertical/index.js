const navigation = () => {
  return [

    // {
    //   title: 'Dashboards',
    //   icon: 'mdi:home-outline',
    //   badgeContent: 'new',
    //   badgeColor: 'error',
    //   children: [
    //     {
    //       title: 'CRM',
    //       path: '/dashboards/crm'
    //     },
    //     {
    //       title: 'Analytics',
    //       path: '/dashboards/analytics'
    //     },
    //     {
    //       title: 'eCommerce',
    //       path: '/dashboards/ecommerce'
    //     }
    //   ]
    // },

   // Employee Dashboard
   {
    title: 'Employee Dashboards',
    icon: 'mdi:home-outline',
    badgeContent: 'new',
    badgeColor: 'error',
    path: '/dashboards/employee-dashboard'
    
  },

  {
title:'Note HR Location',
icon:'mdi:pen',
path:'/dashboards/note_hr_location'
  },
   // HR
   {
    title: 'HR Portal',
    icon: 'mdi:home-outline',
    badgeColor: 'error',
    children: [
      // HR
    {
      title: 'HR',
      icon: 'mdi:home-outline',
      badgeColor: 'error',
      children: [
        
        {
          title: 'Home',
          path: '/dashboards/hr/home'
        },

        {
          title: 'HR Direct',
          path: '/dashboards/hr/hr-direct'
        },

        {
          title: 'Work Structure',
          path: '/dashboards/workStructureHrModule'
        },
       
        {
          title: 'Reports',
          path: '/dashboards/hr/reports'
        },
        {
          title: 'Master Data',
          path: '/dashboards/hr/master-data'
        }
      ]
    },

      {
        title: 'HR 2 ',
        icon: 'mdi:phone',
        badgeColor: 'error',
        children: [
          {
            title: 'Employee Dashboard',
            path: '/dashboards/hr-dashboard/home'
          },

          {
            title: 'Employee Profile',
            path: '/dashboards/employee-dashboard'
          }
        ]
      },

      {
        title: 'HR Only',
        icon: 'mdi:chart-donut',
        children: [
          {
            title: 'Termination',
            path: '/dashboards/hr-only/termination'
          },
          {
            title: 'Remote Sign In/Out',
            path: '/dashboards/hr-only/remote-sign-in-out'
          }
        ]
      }

    
    ]
  },

//Employee Profile
{
  title: 'Employee Profile',
  icon: 'mdi:account-outline',
  badgeColor: 'error',
  path: '/dashboards/employee-profile'
  
},
    // Document Management
    {
      title: 'Document Management',
      icon: 'mdi:account-outline',
      children: [
        {
          title: 'Workspace',
          path: '/dashboards/document-management/workspace'
        },
        {
          title: 'Documents',
          path: '/dashboards/document-management/documents'
        },
      ]
    },

   // Work Structure
   {
    title: 'Work Structure',
    icon: 'mdi:file-document-outline',
    children: [
      {
        title: 'HR Policy Document',
        path: '/apps/invoice/list'
      },
      {
        title: 'HR Location',
        path: '/apps/invoice/preview'
      },
      {
        title: 'HR Country',
        path: '/apps/invoice/edit'
      },
      {
        title: 'HR Nationality',
        path: '/apps/invoice/add'
      },
      {
        title: 'HR Sponsor',
        path: '/dashboards/workStructure/sponsor'
      },
      {
        title: 'HR City',
        path: '/dashboards/workStructure/City'
      },
      {
        title: 'Grade',
        path: '/dashboards/workStructure/Grade'
      },
      {
        title: 'Job',
        path: '/dashboards/workStructure/Job'
      },
      {
        title: 'Cost Center',
        path: '/dashboards/workStructure/Costcenter'
      },
      {
        title: 'Department',
        path: '/dashboards/workStructure/Department'
      },
      {
        title: 'Department Hierarchy',
        path:  '/dashboards/workStructure/Department_H'
      },
      {
        title: 'Position',
        path: '/dashboards/workStructure/position'
      },
      {
        title: 'Position Hierarchy',
        path: '/dashboards/workStructure/position_Hirarachy'
      },
      {
        title: 'Person',
        path: '/dashboards/workStructure/person'
      },
      {
        title: 'Contract',
        path: '/dashboards/workStructure/contact'
      },
      {
        title: 'Assignment',
        path: '/dashboards/workStructure/Assignment'
      }
    ]
  },




    //Hierachy Chaert
    {
      title: 'Hierarchy Chart',
      icon: 'mdi:account-outline',
      children: [
        {
          title: 'Position Hierarchy',
          // path:  '/dashboards/hierarchy-chart/position-hierarchy'
        },
        {
          title: 'Department Hierarchy',
          path: '/dashboards/hierarchy-chart/department-hierarchy'
        },
        {
          title: 'Business Hierarchy',
          path: '/dashboards/hierarchy-chart/business-hierarchy'
        },
        {
          title: 'Approval Hierarchy',
          path: '/dashboards/hierarchy-chart/approval-hierarchy'
        },
        // {
        //   title: 'Department Hierarchy',
        //   children: [
        //     {
        //       title: 'Overview',
        //       path: '/apps/user/view/overview'
        //     },
        //     {
        //       title: 'Security',
        //       path: '/apps/user/view/security'
        //     },
        //     {
        //       title: 'Billing & Plans',
        //       path: '/apps/user/view/billing-plan'
        //     },
        //     {
        //       title: 'Notifications',
        //       path: '/apps/user/view/notification'
        //     },
        //     {
        //       title: 'Connection',
        //       path: '/apps/user/view/connection'
        //     }
        //   ]
        // }
      ]
    },


   //Leave
   {
    title: 'Leave',
    icon: 'mdi:account-edit',
    children: [
     {
        title: 'Leave Details',
        path: '/dashboards/leave/leave-details'
      },
      {
        title: 'Bussiness Trip',
        path: '/dashboards/leave/bussiness-trip'
      },
      {
        title: 'Time Permission',
        path: '/dashboards/leave/time-permison'
      }
    ]
  },

  //Payroll
  {
    title: 'Payroll',
    icon: 'mdi:file-document-outline',
    path: '/dashboards/payroll'
    // children: [
    //   {
    //     title: 'Salary Info',
    //     path: '//faq'
    //   },
    //   {
    //     title: 'PaySlip',
    //     path: '/pages/help-center'
    //   },

    // ]
  },

    //Attendance
    {
      title: 'Attendance',
      icon: 'mdi:file-document-outline',
      children: [
        {
          title: 'Remote Sign In/Out Request',
          path: 
            '/dashboards/attendance/Remote-Sign-in-out'
        },
        {
          title: 'Attendance Details',
          path: '/dashboards/attendance/AttendanceDetails'
        },
        {
          title: 'Access Logs',
          path: '/dashboards/attendance/AccessLogs'        
        },
        {
          title: 'Router Schedule',
          path: '/dashboards/attendance/RosterSchedule'
        }
        
      ]
    },

    //Work List
       {
        title: 'WorkList',
        icon: 'mdi:file-document-outline',
        children: [
          {
            title: 'Dashboard',
            path: '/dashboards/worklist/dashboard',
          },
          {
            title: 'My Services',
            path: '/dashboards/worklist/myservices',
          },
          {
            title: 'My Tasks',
            path: '/dashboards/worklist/mytasks',
          },
          {
            title: 'My Notes',
            path: '/dashboards/worklist/mynotes',
          }
          
        ]
      },
  


    //Help Disk
    {
      title: 'Help Desk',
      icon: 'mdi:file-document-outline',
      children: [
        {
          title: 'Helpdesk Dashboard',
          path: '/dashboards/help-desk-main/MydashBoard/MydashBoard'
        },
        {
          title: 'My Requests',
          path: '/dashboards/help-desk-main/MyRequest/MyRequest'
        },
        {
          title: 'My Tasks',
          path: '/dashboards/help-desk-main/Mytask/Mytask'
        }
       
        
      ]
    },

    // Policy Document
    {
      icon: 'mdi:vector-arrange-below',
      title: 'Policy Documents',
children:[
  {
    title:'HR Policy Document',
    path:'/dashboards/policy_document/hr_policy'
  }
]
    },
    // App & Pages
    // {
    //   sectionTitle: 'Apps & Pages'
    // },
    // {
    //   title: 'Email',
    //   icon: 'mdi:email-outline',
    //   path: '/apps/email'
    // },
    
    // {
    //   title: 'Chat',
    //   icon: 'mdi:message-outline',
    //   path: '/apps/chat'
    // },

    // {
    //   title: 'Calendar',
    //   icon: 'mdi:calendar-blank-outline',
    //   path: '/apps/calendar'
    // },
    // {
    //   title: 'Invoice',
    //   icon: 'mdi:file-document-outline',
    //   children: [
    //     {
    //       title: 'List',
    //       path: '/apps/invoice/list'
    //     },
    //     {
    //       title: 'Preview',
    //       path: '/apps/invoice/preview'
    //     },
    //     {
    //       title: 'Edit',
    //       path: '/apps/invoice/edit'
    //     },
    //     {
    //       title: 'Add',
    //       path: '/apps/invoice/add'
    //     }
    //   ]
    // },
    // {
    //   title: 'User',
    //   icon: 'mdi:account-outline',
    //   children: [
    //     {
    //       title: 'List',
    //       path: '/apps/user/list'
    //     },
    //     {
    //       title: 'View',
    //       children: [
    //         {
    //           title: 'Overview',
    //           path: '/apps/user/view/overview'
    //         },
    //         {
    //           title: 'Security',
    //           path: '/apps/user/view/security'
    //         },
    //         {
    //           title: 'Billing & Plans',
    //           path: '/apps/user/view/billing-plan'
    //         },
    //         {
    //           title: 'Notifications',
    //           path: '/apps/user/view/notification'
    //         },
    //         {
    //           title: 'Connection',
    //           path: '/apps/user/view/connection'
    //         }
    //       ]
    //     }
    //   ]
    // },
    // {
    //   title: 'Roles & Permissions',
    //   icon: 'mdi:shield-outline',
    //   children: [
    //     {
    //       title: 'Roles',
    //       path: '/apps/roles'
    //     },
    //     {
    //       title: 'Permissions',
    //       path: '/apps/permissions'
    //     }
    //   ]
    // },
    // {
    //   title: 'Pages',
    //   icon: 'mdi:file-document-outline',
    //   children: [
    //     {
    //       title: 'User Profile',
    //       children: [
    //         {
    //           title: 'Profile',
    //           path: '/pages/user-profile/profile'
    //         },
    //         {
    //           title: 'Teams',
    //           path: '/pages/user-profile/teams'
    //         },
    //         {
    //           title: 'Projects',
    //           path: '/pages/user-profile/projects'
    //         },
    //         {
    //           title: 'Connections',
    //           path: '/pages/user-profile/connections'
    //         }
    //       ]
    //     },
    //     {
    //       title: 'Account Settings',
    //       children: [
    //         {
    //           title: 'Account',
    //           path: '/pages/account-settings/account'
    //         },
    //         {
    //           title: 'Security',
    //           path: '/pages/account-settings/security'
    //         },
    //         {
    //           title: 'Billing',
    //           path: '/pages/account-settings/billing'
    //         },
    //         {
    //           title: 'Notifications',
    //           path: '/pages/account-settings/notifications'
    //         },
    //         {
    //           title: 'Connections',
    //           path: '/pages/account-settings/connections'
    //         }
    //       ]
    //     },
    //     {
    //       title: 'FAQ',
    //       path: '/pages/faq'
    //     },
    //     {
    //       title: 'Help Center',
    //       path: '/pages/help-center'
    //     },
    //     {
    //       title: 'Pricing',
    //       path: '/pages/pricing'
    //     },
    //     {
    //       title: 'Miscellaneous',
    //       children: [
    //         {
    //           openInNewTab: true,
    //           title: 'Coming Soon',
    //           path: '/pages/misc/coming-soon'
    //         },
    //         {
    //           openInNewTab: true,
    //           title: 'Under Maintenance',
    //           path: '/pages/misc/under-maintenance'
    //         },
    //         {
    //           openInNewTab: true,
    //           title: 'Page Not Found - 404',
    //           path: '/pages/misc/404-not-found'
    //         },
    //         {
    //           openInNewTab: true,
    //           title: 'Not Authorized - 401',
    //           path: '/pages/misc/401-not-authorized'
    //         },
    //         {
    //           openInNewTab: true,
    //           title: 'Server Error - 500',
    //           path: '/pages/misc/500-server-error'
    //         }
    //       ]
    //     }
    //   ]
    // },
    {
      title: 'Auth Pages',
      icon: 'mdi:lock-outline',
      children: [
        {
          title: 'Login',
          children: [
            {
              openInNewTab: true,
              title: 'Login v1',
              path: '/pages/auth/login-v1'
            },
            {
              openInNewTab: true,
              title: 'Login v2',
              path: '/pages/auth/login-v2'
            },
            {
              openInNewTab: true,
              title: 'Login With AppBar',
              path: '/pages/auth/login-with-appbar'
            }
          ]
        },
        {
          title: 'Register',
          children: [
            {
              openInNewTab: true,
              title: 'Register v1',
              path: '/pages/auth/register-v1'
            },
            {
              openInNewTab: true,
              title: 'Register v2',
              path: '/pages/auth/register-v2'
            },
            {
              openInNewTab: true,
              title: 'Register Multi-Steps',
              path: '/pages/auth/register-multi-steps'
            }
          ]
        },
        {
          title: 'Verify Email',
          children: [
            {
              openInNewTab: true,
              title: 'Verify Email v1',
              path: '/pages/auth/verify-email-v1'
            },
            {
              openInNewTab: true,
              title: 'Verify Email v2',
              path: '/pages/auth/verify-email-v2'
            }
          ]
        },
        {
          title: 'Forgot Password',
          children: [
            {
              openInNewTab: true,
              title: 'Forgot Password v1',
              path: '/pages/auth/forgot-password-v1'
            },
            {
              openInNewTab: true,
              title: 'Forgot Password v2',
              path: '/pages/auth/forgot-password-v2'
            }
          ]
        },
        {
          title: 'Reset Password',
          children: [
            {
              openInNewTab: true,
              title: 'Reset Password v1',
              path: '/pages/auth/reset-password-v1'
            },
            {
              openInNewTab: true,
              title: 'Reset Password v2',
              path: '/pages/auth/reset-password-v2'
            }
          ]
        },
        {
          title: 'Two Steps',
          children: [
            {
              openInNewTab: true,
              title: 'Two Steps v1',
              path: '/pages/auth/two-steps-v1'
            },
            {
              openInNewTab: true,
              title: 'Two Steps v2',
              path: '/pages/auth/two-steps-v2'
            }
          ]
        }
      ]
    },



    {
      title: 'Self Service',
      icon: 'mdi:home',
      children: [
        {
          title: 'Resignation',
          path: '/dashboards/self-service/resignation'
        },
        {
          title: 'Misconducts',
          path: '/dashboards/self-service/misconducts'
        },
        {
          title: 'Transfer Request',
          path: '/dashboards/self-service/transfer-request'
        }
      ]
    },
    {
      icon: 'mdi:vector-arrange-below',
      title: 'Dialog Examples',
      path: '/pages/dialog-examples'
    },

    // {
    //   sectionTitle: 'User Interface'
    // },

    //Typography
    // {
    //   title: 'Typography',
    //   icon: 'mdi:format-letter-case',
    //   path: '/ui/typography'
    // },
    // {
    //   title: 'Icons',
    //   path: '/ui/icons',
    //   icon: 'mdi:google-circles-extended'
    // },
    // {
    //   title: 'Cards',
    //   icon: 'mdi:credit-card-outline',
    //   children: [
    //     {
    //       title: 'Basic',
    //       path: '/ui/cards/basic'
    //     },
    //     {
    //       title: 'Advanced',
    //       path: '/ui/cards/advanced'
    //     },
    //     {
    //       title: 'Statistics',
    //       path: '/ui/cards/statistics'
    //     },
    //     {
    //       title: 'Widgets',
    //       path: '/ui/cards/widgets'
    //     },
    //     {
    //       title: 'Gamification',
    //       path: '/ui/cards/gamification'
    //     },
    //     {
    //       title: 'Actions',
    //       path: '/ui/cards/actions'
    //     }
    //   ]
    // },
    // {
    //   badgeContent: '18',
    //   title: 'Components',
    //   icon: 'mdi:archive-outline',
    //   badgeColor: 'primary',
    //   children: [
    //     {
    //       title: 'Accordion',
    //       path: '/components/accordion'
    //     },
    //     {
    //       title: 'Alerts',
    //       path: '/components/alerts'
    //     },
    //     {
    //       title: 'Avatars',
    //       path: '/components/avatars'
    //     },
    //     {
    //       title: 'Badges',
    //       path: '/components/badges'
    //     },
    //     {
    //       title: 'Buttons',
    //       path: '/components/buttons'
    //     },
    //     {
    //       title: 'Button Group',
    //       path: '/components/button-group'
    //     },
    //     {
    //       title: 'Chips',
    //       path: '/components/chips'
    //     },
    //     {
    //       title: 'Dialogs',
    //       path: '/components/dialogs'
    //     },
    //     {
    //       title: 'List',
    //       path: '/components/list'
    //     },
    //     {
    //       title: 'Menu',
    //       path: '/components/menu'
    //     },
    //     {
    //       title: 'Pagination',
    //       path: '/components/pagination'
    //     },
    //     {
    //       title: 'Ratings',
    //       path: '/components/ratings'
    //     },
    //     {
    //       title: 'Snackbar',
    //       path: '/components/snackbar'
    //     },
    //     {
    //       title: 'Swiper',
    //       path: '/components/swiper'
    //     },
    //     {
    //       title: 'Tabs',
    //       path: '/components/tabs'
    //     },
    //     {
    //       title: 'Timeline',
    //       path: '/components/timeline'
    //     },
    //     {
    //       title: 'Toasts',
    //       path: '/components/toast'
    //     },
    //     {
    //       title: 'Tree View',
    //       path: '/components/tree-view'
    //     },
    //     {
    //       title: 'More',
    //       path: '/components/more'
    //     },
    //   ]
    // },

    // Form & Tables
    // {
    //   sectionTitle: 'Forms & Tables'
    // },
    // {
    //   title: 'Form Elements',
    //   icon: 'mdi:form-select',
    //   children: [
    //     {
    //       title: 'Text Field',
    //       path: '/forms/form-elements/text-field'
    //     },
    //     {
    //       title: 'Select',
    //       path: '/forms/form-elements/select'
    //     },
    //     {
    //       title: 'Checkbox',
    //       path: '/forms/form-elements/checkbox'
    //     },
    //     {
    //       title: 'Radio',
    //       path: '/forms/form-elements/radio'
    //     },
    //     {
    //       title: 'Custom Inputs',
    //       path: '/forms/form-elements/custom-inputs'
    //     },
    //     {
    //       title: 'Textarea',
    //       path: '/forms/form-elements/textarea'
    //     },
    //     {
    //       title: 'Autocomplete',
    //       path: '/forms/form-elements/autocomplete'
    //     },
    //     {
    //       title: 'Date Pickers',
    //       path: '/forms/form-elements/pickers'
    //     },
    //     {
    //       title: 'Switch',
    //       path: '/forms/form-elements/switch'
    //     },
    //     {
    //       title: 'File Uploader',
    //       path: '/forms/form-elements/file-uploader'
    //     },
    //     {
    //       title: 'Editor',
    //       path: '/forms/form-elements/editor'
    //     },
    //     {
    //       title: 'Slider',
    //       path: '/forms/form-elements/slider'
    //     },
    //     {
    //       title: 'Input Mask',
    //       path: '/forms/form-elements/input-mask'
    //     },
    //   ]
    // },
    // {
    //   icon: 'mdi:cube-outline',
    //   title: 'Form Layouts',
    //   path: '/forms/form-layouts'
    // },
    // {
    //   title: 'Form Validation',
    //   path: '/forms/form-validation',
    //   icon: 'mdi:checkbox-marked-circle-outline'
    // },
    // {
    //   title: 'Form Wizard',
    //   path: '/forms/form-wizard',
    //   icon: 'mdi:transit-connection-horizontal'
    // },
    // {
    //   title: 'Table',
    //   icon: 'mdi:grid-large',
    //   path: '/tables/mui'
    // },
    // {
    //   title: 'Mui DataGrid',
    //   icon: 'mdi:grid',
    //   path: '/tables/data-grid'
    // },

    // Charts
    {
      sectionTitle: 'Charts & Misc'
    },
    {
      title: 'Reimbursement',
      icon: 'mdi:chart-donut',
      children: [
        {
          title: 'Travel Reimbursement',
          path: '/dashboards/reimbursement/travel-reimbursement'
        },
        {
          title: 'Medical Reimbursement',
          path: '/dashboards/reimbursement/medical-reimbursement'
        },
        {
          title: 'Educational Reimbursement',
          path: '/dashboards/reimbursement/educational-reimbursement'
        },
        {
          title: 'Other Reimbursement',
          path: '/dashboards/reimbursement/other-reimbursement'
        }
      ]
    },
    {
      path: '/acl',
      action: 'read',
      subject: 'acl-page',
      icon: 'mdi:shield-outline',
      title: 'Access Control'
    },
    {
      title: 'Others',
      icon: 'mdi:dots-horizontal',
      children: [
        {
          title: 'Menu Levels',
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
          disabled: true
        },
        {
          title: 'Raise Support',
          externalLink: true,
          openInNewTab: true,
          path: 'https://themeselection.com/support'
        },
        {
          title: 'Documentation',
          externalLink: true,
          openInNewTab: true,
          path: 'https://demos.themeselection.com/marketplace/materio-mui-react-nextjs-admin-template/documentation'
        },

           
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
    }
      ]
    }

  ]
}

export default navigation
