// ** MUI Imports
import Grid from '@mui/material/Grid'

// ** Styled Component Import
import ApexChartWrapper from 'src/@core/styles/libs/react-apexcharts'

// ** Demo Components Imports
import EmployeeDashboard from '../../employee-dashboard'
 

const HrDirectDashboard = () => {
  return (
    <ApexChartWrapper>
      <Grid container spacing={6}>
          <EmployeeDashboard/>
      </Grid>
    </ApexChartWrapper>
  )
}

export default HrDirectDashboard 
