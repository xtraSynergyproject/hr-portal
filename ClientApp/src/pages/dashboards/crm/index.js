// ** MUI Imports
import Grid from '@mui/material/Grid'

// ** Icon Imports
import Icon from 'src/@core/components/icon'

// ** Custom Components Imports
import CardStatisticsCharacter from 'src/@core/components/card-statistics/card-stats-with-image'
import CardStatisticsVerticalComponent from 'src/@core/components/card-statistics/card-stats-vertical'

// ** Styled Component Import
import ApexChartWrapper from 'src/@core/styles/libs/react-apexcharts'

// ** Demo Components Imports

import CrmRevenueReport from 'src/views/dashboards/crm/CrmRevenueReport'
import CrmSalesOverview from 'src/views/dashboards/crm/CrmSalesOverview'

import EmployeeDashboardUserProfile from 'src/views/dashboards/employee-dashboard/EmployeDashboardUserProfile'

import EmployeeDashboardAdministrationCard from 'src/views/dashboards/employee-dashboard/EmployeeDashboardAdministratorCard'


const data = [
  {
    // stats: '13',
    id: 1,
    title: 'Leave Request',
    chipColor: 'primary',
    chipText: 'See More',
    src: '/images/cards/pose_f9.png'
  },
  {
    // stats: '24',
    id: 2,
    trend: 'negative',
    title: 'Request Time Off',
    chipText: 'See More',
    chipColor: 'secondary',
    src: '/images/cards/pose_m18.png'
  },

  {
    // stats: '13',
    id: 3,
    title: 'Employee Directory',
    chipColor: 'primary',
    chipText: 'See More',
    src: '/images/cards/pose_f9.png'
  },
  {
    // stats: '24',
    id: 4,
    trend: 'negative',
    title: 'My Profile',
    chipText: 'See More',
    chipColor: 'secondary',
    src: '/images/cards/pose_m18.png'
  }

]

const EmployeeDashboard = () => {
  return (
    <ApexChartWrapper>
      <Grid container spacing={6}>
        
        <Grid item xs={12}  >
          <EmployeeDashboardUserProfile/>
        </Grid>

        <Grid item xs={12}  >
          <h3>  Shortcuts </h3>
        </Grid>
        {
          data.map((elem, i) => {
            return <Grid id={i} item xs={12} sm={6} md={3} sx={{ pt: theme => `${theme.spacing(12.25)} !important` }}>
              <CardStatisticsCharacter data={elem} />
            </Grid>

          })
        }

        <Grid item xs={12} sm={6} md={4}>
          <h3>You Report To </h3>
          < EmployeeDashboardAdministrationCard />
        </Grid>
        <Grid item xs={12} sm={6} md={4}>
          <h3> My Tasks </h3>
          <CrmRevenueReport />
        </Grid>
        <Grid item xs={12} sm={6} md={4}>
          <h3>My Services </h3>
          <CrmSalesOverview />
        </Grid>
      </Grid>
    </ApexChartWrapper>
  )
}

export default EmployeeDashboard
