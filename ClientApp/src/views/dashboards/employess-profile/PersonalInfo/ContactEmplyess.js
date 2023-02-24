

import { useState, useEffect } from 'react'
// ** MUI Components
import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import Typography from '@mui/material/Typography'
import Model from '../../../../views/dashboards/workstructure/Components (1)/Components/Model'
// ** api Imports
import axios from 'axios'


const UserProfileHeader = () => {
  // ** State
  const [data, setData] = useState(null)
  useEffect(() => {
    axios.get('/pages/profile-header').then(response => {
      setData(response.data)
    })
  }, [])


  // ** State
  const [datta, seetData] = useState(null)
  useEffect(() => {
    axios
      .get(
        'https://webapidev.aitalkx.com/chr/hrdirect/Contract?userId=60da8f8f195197515042a1f2&personId=0a11a928-4f66-41b4-aa44-150d1470ef7e'
      )
      .then(response => {
        setData(response.data)
        localStorage.setItem('userProfile', JSON.stringify(response.data))
        console.log(response.data, 'profile data')
      })
  }, [])
  console.log(data, 'data')
  const designationIcon = data?.designationIcon || 'mdi:briefcase-outline'

  return data !== null ? (
    <>
      <Card sx={{ display: "flex" }} className="user_profile_grid">
      <Box sx={{ minWidth: '60%', m: 2, }}>
          <Typography sx={{ display: 'flex', mx: 2, mb: 2, fontSize: 18 }}>
            {/* <PersonOutlinedIcon /> */}
            <Box sx={{ ml: 2 }}>
              <h4>Contract</h4>
            </Box>
          </Typography>
        </Box>
        <Box sx={{ m: 2, }}>
          <Typography sx={{ display: 'flex', m: 2, }}>

            <Box >
              <Model />
            </Box>
          </Typography>
        </Box>
      </Card>


      <Card sx={{ py: "10px", display: "flex" }} className="user_profile_grid">
        <Box sx={{ minWidth: '6%', display: "flex", justifyContent: "center" }}>

        </Box>

        <Box className='user_profile_box box_one' sx={{ minWidth: '20%', mb: 25 }}>
          <Typography sx={{ display: 'flex', m: 2}}>

            <Box sx={{ ml: 3 }}>
              ContractType:
            </Box>

          </Typography>

          <Typography sx={{ display: 'flex', m: 2 }}>

            <Box sx={{ ml: 3 }}>

              Sponsor:
            </Box>

          </Typography>




        </Box>
        <Box className='user_profile_box box_two' sx={{ minWidth: '20%' }}>
          <Typography sx={{ display: 'flex', m: 2 }}>

            <Box sx={{ ml: 3 }}>
              <b>{data.ContractType} </b>
            </Box>
          </Typography>

          <Typography sx={{ display: 'flex', m: 2 }}>

            <Box sx={{ ml: 3 }}>
              <b>{data.SponsorName} </b>
            </Box>
          </Typography>

       
         
        </Box>
        <Box className='user_profile_box box_two' sx={{ minWidth: '15%' }}>
          <Typography sx={{ display: 'flex', m: 2 }}>

            <Box sx={{ ml: 3 }}>
               ContractRenewable:
            </Box>
          </Typography>

          <Typography sx={{ display: 'flex', m: 2 }}>

            <Box sx={{ ml: 3 }}>
              AnnualLeaveEntitlement:
            </Box>
          </Typography>
          </Box>

        <Box className='user_profile_box box_two' sx={{ minWidth: '20%' }}>
          <Typography sx={{ display: 'flex', m: 2 }}>

            <Box sx={{ ml: 3 }}>
              <b>{data.ContractRenewable} </b>
            </Box>
          </Typography>

          <Typography sx={{ display: 'flex', m: 2 }}>

            <Box sx={{ ml: 3 }}>
              <b>{data.AnnualLeaveEntitlement} </b>
            </Box>
          </Typography>

      
        </Box>

      </Card>
      <Box sx={{ width: '100%', display: 'flex', ml: { xs: 0, md: 6 }, alignItems: 'flex-end', flexWrap: ['wrap', 'nowrap'], justifyContent: ['center', 'space-between'] }}>


      </Box>
    </>


  ) : null
}

export default UserProfileHeader



